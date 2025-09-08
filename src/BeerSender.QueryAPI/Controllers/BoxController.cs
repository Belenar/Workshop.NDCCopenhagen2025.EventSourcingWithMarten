using BeerSender.Domain.Boxes;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace BeerSender.QueryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BoxController(IDocumentStore store) : ControllerBase
{
    // [HttpGet]
    // [Route("{id:guid}")]
    // public async Task<Box?> GetById([FromRoute]Guid id)
    // {
    //
    // }
    //
    // [HttpGet]
    // [Route("{id:guid}/by-sequence/{sequence:int}")]
    // public async Task<Box?> GetById([FromRoute] Guid id, [FromRoute]int sequence)
    // {
    //
    // }
    //
    // [HttpGet]
    // [Route("all-open")]
    // public async Task<IEnumerable<OpenBox>> GetOpenBoxes()
    // {
    //
    // }
    //
    // [HttpGet]
    // [Route("all-unsent")]
    // public async Task<IEnumerable<UnsentBox>> GetUnsentBoxes()
    // {
    //
    // }
}