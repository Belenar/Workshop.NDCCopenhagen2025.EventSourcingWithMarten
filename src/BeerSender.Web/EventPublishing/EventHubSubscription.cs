using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using Marten.Subscriptions;
using Microsoft.AspNetCore.SignalR;

namespace BeerSender.Web.EventPublishing;

public class EventHubSubscription(IHubContext<EventHub> HubContext) 
    : ISubscription
{
    public Task<IChangeListener> ProcessEventsAsync(
        EventRange page, 
        ISubscriptionController controller, 
        IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        foreach (var @event in page.Events)
        {
            HubContext.Clients.Group(@event.StreamId.ToString())
                .SendAsync("PublishEvent", 
                    @event.StreamId, @event.Data, @event.Data.GetType().Name);
        }

        return Task.FromResult(NullChangeListener.Instance);
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask();
    }
}