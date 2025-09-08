using BeerSender.Domain.Boxes.Events;
using Marten;

namespace BeerSender.Domain.Boxes.Commands;

public record CreateBox(
    Guid BoxId,
    int DesiredNumberOfSpots
) : ICommand;

public class CreateBoxHandler
    : ICommandHandler<CreateBox>
{
    public async Task Handle(IDocumentSession session, CreateBox command)
    {
        var capacity = BoxCapacity.Create(command.DesiredNumberOfSpots);
        var @event = new BoxCreated(capacity);
        session.Events.StartStream<Box>(command.BoxId, @event);
    }
}