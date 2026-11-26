using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOA_CA2.DTOs;
using SOA_CA2.Models;

namespace SOA_CA2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PerformerController : ControllerBase
{
    private readonly IPerformerRepository _repository;

    public PerformerController(IPerformerRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PerformerDto>>> GetPerformers(CancellationToken cancellationToken)
    {
        var performers = await _repository.ListAsync(cancellationToken);
        return Ok(performers.Select(performer => new PerformerDto(performer.Id, performer.Name)));
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PerformerDto>> GetPerformer(int id, CancellationToken cancellationToken)
    {
        var performer = await _repository.FindAsync(id, cancellationToken);
        if (performer == null)
        {
            return NotFound();
        }

        return new PerformerDto(performer.Id, performer.Name);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PerformerDto>> CreatePerformer(
        PerformerRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var performer = new Performer
        {
            Name = request.Name.Trim(),
            OrganiserId = organiserId
        };

        await _repository.AddAsync(performer, cancellationToken);

        return CreatedAtAction(nameof(GetPerformer), new { id = performer.Id },
            new PerformerDto(performer.Id, performer.Name));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePerformer(
        int id, PerformerRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var updated = await _repository.UpdateAsync(id, organiserId, request.Name.Trim(), cancellationToken);
        return updated ? NoContent() : NotFound();
    }
}
