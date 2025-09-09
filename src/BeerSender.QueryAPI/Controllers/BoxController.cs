using BeerSender.Domain.Boxes;
using BeerSender.Domain.Projections;
using Marten;
using Marten.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace BeerSender.QueryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BoxController(IDocumentStore store) : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<Box?> GetById([FromRoute]Guid id)
    {
        await using var session = store.IdentitySession();
        // Loads the document from the event stream
        // var box = await session.Events.AggregateStreamAsync<Box>(id);
        // Loads the doc from the document collection
        // var box = await session.LoadAsync<Box>(id);
        
        var snapshot = await session.LoadAsync<Box>(id);

        var box = await session.Events.AggregateStreamAsync(
            id,
            fromVersion: (snapshot is null ? 0 : snapshot.Version + 1),
            state: snapshot);
        return box;
    }

    [HttpGet]
    [Route("{id:guid}/by-sequence/{sequence:int}")]
    public async Task<Box?> GetById([FromRoute] Guid id, [FromRoute]int sequence)
    {
        await using var session = store.QuerySession();
        var box = await session.Events.AggregateStreamAsync<Box>(id, version: sequence);
        return box;
    }
    
    [HttpGet]
    [Route("all-open")]
    public async Task<IEnumerable<OpenBox>> GetOpenBoxes()
    {
        await using var session = store.QuerySession();
        var boxes = await session.Query<OpenBox>()
            // LINQ can go here
            .ToListAsync();
        return boxes;
    }
    
    [HttpGet]
    [Route("all-unsent")]
    public async Task<IEnumerable<UnsentBox>> GetUnsentBoxes()
    {
        await using var session = store.QuerySession();
        var boxes = await session.Query<UnsentBox>()
            // LINQ can go here
            .ToListAsync();
        return boxes;
    }
}