using BeerSender.Domain.Boxes;
using Marten.Events.Projections;

namespace BeerSender.Domain.Projections;

public class BeerInBoxes
{
    public required string BottleType { get; set; }
    public required BeerBottle Bottle { get; set; }
    public List<Guid> BoxIds { get; set; } = new();
}

public class BeerInBottlesProjection : MultiStreamProjection<BeerInBoxes, string>
{
    //TODO
}