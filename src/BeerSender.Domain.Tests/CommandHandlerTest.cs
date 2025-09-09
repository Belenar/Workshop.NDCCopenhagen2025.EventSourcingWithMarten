using AwesomeAssertions;
using Marten;

namespace BeerSender.Domain.Tests;

[Collection("Marten collection")]
public abstract class CommandHandlerTest<TCommand>
    where TCommand : class, ICommand
{
    public CommandHandlerTest(MartenFixture martenFixture)
    {
        Store = martenFixture.Store;
    }

    private Dictionary<Guid, long> _streamVersions = new();
    protected IDocumentStore Store { get; set; }
    
    protected readonly Guid _aggregateId = Guid.NewGuid();
    
    protected abstract ICommandHandler<TCommand> Handler { get; }

    protected async Task Given<TAggregate>(params object[] events)
        where TAggregate : class
    {
        await Given<TAggregate>(_aggregateId, events);
    }

    protected async Task Given<TAggregate>(Guid streamId, params object[] events)
        where TAggregate : class
    {
        if (events.IsEmpty()) return;

        await using var session = Store.LightweightSession();

        var stream = session.Events.StartStream<TAggregate>(streamId, events);
        
        await session.SaveChangesAsync();
        
        _streamVersions[streamId] = stream.Version;
    }

    protected async Task When(TCommand command)
    {
        await using var session = Store.IdentitySession();
        await Handler.Handle(session, command);
        await session.SaveChangesAsync();
    }

    protected async Task Then(params object[] expectedEvents)
    {
        await Then(_aggregateId, expectedEvents);
    }

    protected async Task Then(Guid streamId, params object[] expectedEvents)
    {
        await using var session = Store.QuerySession();

        var version = _streamVersions.ContainsKey(streamId)
            ? _streamVersions[streamId] + 1
            : 0L;

        var storedEvents = await session.Events.FetchStreamAsync(streamId, fromVersion: version);

        var actualEvents = storedEvents
            .OrderBy(e => e.Version)
            .Select(e => e.Data)
            .ToArray();
        
        actualEvents.Length.Should().Be(expectedEvents.Length);

        for (int i = 0; i < expectedEvents.Length; i++)
        {
            actualEvents[i].Should().BeOfType(expectedEvents[i].GetType());

            try
            {
                actualEvents[i].Should().BeEquivalentTo(expectedEvents[i]);
            }
            catch (InvalidOperationException e)
            {
                if (!e.Message.StartsWith("No members were found for comparison."))
                    throw;
            }
        }
    }
}