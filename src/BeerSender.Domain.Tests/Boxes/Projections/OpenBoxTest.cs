using AwesomeAssertions;
using BeerSender.Domain.Boxes;
using BeerSender.Domain.Boxes.Events;
using BeerSender.Domain.Projections;
using JasperFx.Core;
using Marten.Events;

namespace BeerSender.Domain.Tests.Boxes.Projections;

public class OpenBoxTest(MartenFixture fixture) : MartenTest(fixture)
{
    [Fact]
    public async Task WhenBoxIsOpenWithBottles_OpenBoxShouldBeCorrect()
    {
        var boxId = Guid.NewGuid();

        object[] events =
        [
            Box_created_with_capacity(24),
            Beer_bottle_added(gouden_carolus),
            Beer_bottle_added(carte_blanche)
        ];

        // Create a stream with events
        await using var session = Store.LightweightSession();
        session.Events.StartStream<Box>(boxId, events);
        await session.SaveChangesAsync();

        // Wait for projections to finish
        await Store.WaitForNonStaleProjectionDataAsync(5.Seconds());

        // Query it back
        await using var query = Store.QuerySession();
        var openBox = await query.LoadAsync<OpenBox>(boxId);
        
        // Check the result
        openBox.Should().NotBeNull();
        openBox.BoxId.Should().Be(boxId);
        openBox.EmptySpaces.Should().Be(22);
    }
    
    // Events
    protected BoxCreated Box_created_with_capacity(int capacity)
    {
        return new BoxCreated(new BoxCapacity(capacity));
    }

    protected BeerBottleAdded Beer_bottle_added(BeerBottle bottle)
    {
        return new BeerBottleAdded(bottle);
    }
    
    // Test data
    protected BeerBottle gouden_carolus = new(
        "Gouden Carolus",
        "Quadrupel Whisky Infused",
        12.7,
        BeerType.Quadruple
    );
    
    protected BeerBottle carte_blanche = new(
        "Wolf",
        "Carte Blanche",
        8.5,
        BeerType.Triple
    );
}