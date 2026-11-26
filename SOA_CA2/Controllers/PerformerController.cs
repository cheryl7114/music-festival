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
    [HttpPost]
    public async Task<ActionResult<PerformerDto>> PostPerformer(
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

        return StatusCode(StatusCodes.Status201Created, new PerformerDto(performer.Id, performer.Name));
    }
}
