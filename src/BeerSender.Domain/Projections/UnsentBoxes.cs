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
                BoxId = evt.StreamId
            });
        });
    }
}