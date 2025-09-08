using BeerSender.Domain.Boxes.Events;
using JasperFx.Core.Reflection;
using Marten.Events.Aggregation;

namespace BeerSender.Domain.Projections;

public class OpenBox
{
    public Guid BoxId { get; set; }
    public int EmptySpaces { get; set; }
}

public class OpenBoxProjection : SingleStreamProjection<OpenBox, Guid>
{
    public OpenBoxProjection()
    {
        DeleteEvent<BoxClosed>();
    }
    
    public static OpenBox Create(BoxCreated created)
    {
        return new OpenBox
        {
            EmptySpaces = created.Capacity.NumberOfSpots
        };
    }

    public void Apply(BeerBottleAdded _, OpenBox box)
    {
        box.EmptySpaces--;
    }
}