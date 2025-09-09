using BeerSender.Domain.Boxes;
using BeerSender.Domain.Boxes.Commands;
using BeerSender.Domain.Boxes.Events;

namespace BeerSender.Domain.Tests.Boxes;

public abstract class BoxTest<TCommand>(MartenFixture martenFixture)
    : CommandHandlerTest<TCommand>(martenFixture)
    where TCommand : class, ICommand
{
    // events
    protected BoxCreated Box_created_with_capacity(int capacity)
    {
        return new BoxCreated(new BoxCapacity(capacity));
    }
    
    protected BeerBottleAdded Bottle_was_added(BeerBottle bottle)
    {
        return new BeerBottleAdded(bottle);
    }
    
    // commands
    protected AddBeerBottle Add_beer_bottle(BeerBottle bottle)
    {
        return new AddBeerBottle(_aggregateId, bottle);
    }
    
    // data
    protected BeerBottle carte_blanche = new(
        "Wolf",
        "Carte Blanche",
        8.5,
        BeerType.Triple);
}

public class AddBeerHandlerTest(MartenFixture martenFixture) 
    : BoxTest<AddBeerBottle>(martenFixture)
{
    protected override ICommandHandler<AddBeerBottle> Handler 
        => new AddBeerBottleHandler();

    [Fact]
    public async Task IfBoxIsEmpty_ThenBeerShouldBeAdded()
    {
        await Given<Box>(
            Box_created_with_capacity(6));
        await When(
            Add_beer_bottle(carte_blanche));
        await Then(
            Bottle_was_added(carte_blanche));

    }
}