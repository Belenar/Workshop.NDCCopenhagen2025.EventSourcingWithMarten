using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BeerSender.Domain;

public class CommandRouter(
    IServiceProvider serviceProvider, 
    IDocumentStore store,
    IHttpContextAccessor contextAccessor)
{
    public async Task HandleCommand(ICommand command)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        var handler = serviceProvider.GetRequiredService(handlerType);
        var methodInfo = handlerType.GetMethod("Handle");

        var session = store.IdentitySession();
        
        Guid commandId = Guid.NewGuid();

        StoreCommand(session, commandId, command);
        ConfigureSession(session, commandId);
        
        var handleTask = (Task)methodInfo.Invoke(handler, [session, command] );
        await handleTask;
        
        await session.SaveChangesAsync();
    }

    private void StoreCommand(IDocumentSession session, Guid commandId, ICommand command)
    {
        LoggedCommand loggedCommand = new(
            commandId,
            contextAccessor.HttpContext?.User.Identity?.Name,
            DateTime.UtcNow,
            command);
        
        session.Insert(loggedCommand);
    }

    private void ConfigureSession(IDocumentSession session, Guid commandId)
    {
        session.CausationId = commandId.ToString();
        session.CorrelationId = commandId.ToString(); // Could come from another system (middleware)
        session.SetHeader("TraceIdentifier", contextAccessor.HttpContext?.TraceIdentifier ?? string.Empty);
    }
}

public record LoggedCommand(
    Guid CommandId,
    string? UserName,
    DateTime Timestamp,
    ICommand Command);