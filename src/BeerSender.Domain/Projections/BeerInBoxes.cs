using BeerSender.Domain.Boxes;
using BeerSender.Domain.Boxes.Events;
using JasperFx.Events;
using Marten.Events.Projections;

namespace BeerSender.Domain.Projections;

public class BeerInBoxes
{
    public required string BottleType { get; set; }
    public required BeerBottle Bottle { get; set; }
    public List<Guid> BoxIds { get; set; } = new();
}

public class BeerInBoxesProjection : MultiStreamProjection<BeerInBoxes, string>
{
    public BeerInBoxesProjection()
    {
        Identity<BeerBottleAdded>(x => x.Bottle.BottleId);
    }

    public static BeerInBoxes Create(IEvent<BeerBottleAdded> evt)
    {
        return new BeerInBoxes
        {
            BottleType = evt.Data.Bottle.BottleId,
            Bottle = evt.Data.Bottle,
            BoxIds = [evt.StreamId]
        };
    }
    
    public static void Apply(IEvent<BeerBottleAdded> evt, BeerInBoxes beerInBoxes)
    {
        beerInBoxes.BoxIds.Add(evt.StreamId);
    }
}