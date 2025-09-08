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
        // TO DO
    }
}