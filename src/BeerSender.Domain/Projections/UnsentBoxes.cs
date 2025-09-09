using BeerSender.Domain.Boxes.Events;
using JasperFx.Events;
using Marten.Events.Projections;

namespace BeerSender.Domain.Projections;

public class UnsentBox
{
    public Guid BoxId { get; set; }
    public string? Status { get; set; }
}

public class UnsentBoxProjection : EventProjection
{
    public UnsentBoxProjection()
    {
        Project<IEvent<BoxCreated>>((evt, operations) =>
        {
            operations.Store(new UnsentBox
            {
                BoxId = evt.StreamId,
                Status = "Created"
            });
        });
        
        Project<IEvent<BoxSent>>((evt, operations) =>
        {
            operations.Delete<UnsentBox>(evt.StreamId);
        });
        
        ProjectAsync<IEvent<BoxClosed>>(async (evt, operations, cancellation) =>
        {
            var unsentBox = await operations.LoadAsync<UnsentBox>(evt.StreamId, cancellation);

            if (unsentBox is null) return;

            unsentBox.Status = (unsentBox.Status == "Created") 
                ? "Closed" 
                : "Ready to send!";
            
            operations.Store(unsentBox);
        });
        
        ProjectAsync<IEvent<ShippingLabelAdded>>(async (evt, operations, cancellation) =>
        {
            var unsentBox = await operations.LoadAsync<UnsentBox>(evt.StreamId, cancellation);

            if (unsentBox is null) return;

            unsentBox.Status = (unsentBox.Status == "Created") 
                ? "Label Added" 
                : "Ready to send!";
            
            operations.Store(unsentBox);
        });
    }
}