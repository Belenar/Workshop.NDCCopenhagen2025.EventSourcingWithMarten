using Marten;
using Microsoft.AspNetCore.Http;

namespace BeerSender.Domain;

public class CommandRouter(
    IServiceProvider serviceProvider)
{
    public async Task HandleCommand(ICommand command)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        var handler = serviceProvider.GetService(handlerType);
        var methodInfo = handlerType.GetMethod("Handle");

        // TODO
    }
}