using BeerSender.Domain.Projections;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace BeerSender.QueryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BeerController(IDocumentStore store) : ControllerBase
{
    [HttpGet]
    [Route("in-boxes")]
    public async Task<IEnumerable<BeerInBoxes>> GetOpenBoxes()
    {
        await using var session = store.QuerySession();
        var boxes = await session.Query<BeerInBoxes>()
            // LINQ can go here
            .ToListAsync();
        return boxes;
    }
}