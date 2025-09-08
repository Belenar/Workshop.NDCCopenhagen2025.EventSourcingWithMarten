using Microsoft.AspNetCore.SignalR;

namespace BeerSender.Web.EventPublishing;

public class EventHub : Hub
{
    public async Task PublishEvent(Guid aggregateId, object @event)
    {
        // TODO
    }

    public async Task SubscribeToAggregate(Guid aggregateId)
    {
        // TODO
    }
}