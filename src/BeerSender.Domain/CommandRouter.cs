using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BeerSender.Domain;

public class CommandRouter(
    IServiceProvider serviceProvider, IDocumentStore store)
{
    public async Task HandleCommand(ICommand command)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        var handler = serviceProvider.GetRequiredService(handlerType);
        var methodInfo = handlerType.GetMethod("Handle");

        var session = store.IdentitySession();
        
        var handleTask = (Task)methodInfo.Invoke(handler, [session, command] );
        await handleTask;
        
        await session.SaveChangesAsync();
    }
}