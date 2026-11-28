using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FestivalDto>>> GetFestivals(CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var festivals = await _repository.ListAsync(organiserId, cancellationToken);
        return Ok(festivals.Select(ToDto));
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FestivalDto>> GetFestival(int id, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var festival = await _repository.FindAsync(id, organiserId, cancellationToken);
        if (festival == null)
        {
            return NotFound();
        }

        return ToDto(festival);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<FestivalDto>> CreateFestival(
        FestivalRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var festival = ToFestival(request);
        festival.OrganiserId = organiserId;
        await _repository.AddAsync(festival, cancellationToken);

        return CreatedAtAction(nameof(GetFestival), new { id = festival.Id }, ToDto(festival));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateFestival(
        int id, FestivalRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var festival = ToFestival(request);
        festival.Id = id;
        var updated = await _repository.UpdateAsync(festival, organiserId, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFestival(int id, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var deleted = await _repository.DeleteAsync(id, organiserId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpPost("{id:int}/simulate")]
    public async Task<ActionResult<FestivalSimulationDto>> SimulateFestival(
        int id, FestivalSimulationRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var costs = await _repository.GetCostsAsync(id, organiserId, cancellationToken);
        if (costs == null)
        {
            return NotFound();
        }

        var (bookingFees, expenses) = costs.Value;
        var ticketPrice = request.TicketPrice!.Value;
        var expectedTicketsSold = request.ExpectedTicketsSold!.Value;
        var projectedRevenue = ticketPrice * expectedTicketsSold;
        var totalCosts = bookingFees + expenses;

        return new FestivalSimulationDto(id, ticketPrice, expectedTicketsSold, projectedRevenue,
            bookingFees, expenses, totalCosts, projectedRevenue - totalCosts);
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
