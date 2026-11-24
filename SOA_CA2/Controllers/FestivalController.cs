using Microsoft.AspNetCore.Mvc;
using SOA_CA2.DTOs;
using SOA_CA2.Models;

namespace SOA_CA2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FestivalController : ControllerBase
{
    private readonly IFestivalRepository _repository;

    public FestivalController(IFestivalRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FestivalDto>> GetFestival(int id, CancellationToken cancellationToken)
    {
        var festival = await _repository.FindAsync(id, cancellationToken);
        if (festival == null)
        {
            return NotFound();
        }

        return ToDto(festival);
    }

    [HttpPost]
    public async Task<ActionResult<FestivalDto>> PostFestival(
        CreateFestivalDto request, CancellationToken cancellationToken)
    {
        if (request.EndDate < request.StartDate)
        {
            ModelState.AddModelError(nameof(request.EndDate), "End date must be on or after the start date.");
            return ValidationProblem(ModelState);
        }

        var festival = new Festival
        {
            Name = request.Name.Trim(),
            Location = request.Location.Trim(),
            StartDate = request.StartDate!.Value,
            EndDate = request.EndDate!.Value
        };

        await _repository.AddAsync(festival, cancellationToken);

        return CreatedAtAction(nameof(GetFestival), new { id = festival.Id }, ToDto(festival));
    }

    private static FestivalDto ToDto(Festival festival)
    {
        return new FestivalDto(festival.Id, festival.Name, festival.Location, festival.StartDate, festival.EndDate);
    }
}
