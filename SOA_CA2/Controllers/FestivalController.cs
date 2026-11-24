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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FestivalDto>>> GetFestivals(CancellationToken cancellationToken)
    {
        var festivals = await _repository.ListAsync(cancellationToken);
        return Ok(festivals.Select(ToDto));
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
        FestivalRequestDto request, CancellationToken cancellationToken)
    {
        var festival = ToFestival(request);
        await _repository.AddAsync(festival, cancellationToken);

        return CreatedAtAction(nameof(GetFestival), new { id = festival.Id }, ToDto(festival));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutFestival(
        int id, FestivalRequestDto request, CancellationToken cancellationToken)
    {
        var festival = ToFestival(request);
        festival.Id = id;
        var updated = await _repository.UpdateAsync(festival, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFestival(int id, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static Festival ToFestival(FestivalRequestDto request)
    {
        return new Festival
        {
            Name = request.Name.Trim(),
            Location = request.Location.Trim(),
            StartDate = request.StartDate!.Value,
            EndDate = request.EndDate!.Value
        };
    }

    private static FestivalDto ToDto(Festival festival)
    {
        return new FestivalDto(festival.Id, festival.Name, festival.Location, festival.StartDate, festival.EndDate);
    }
}
