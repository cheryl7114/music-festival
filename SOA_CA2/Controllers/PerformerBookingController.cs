using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOA_CA2.DTOs;
using SOA_CA2.Models;

namespace SOA_CA2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PerformerBookingController : ControllerBase
{
    private readonly IPerformerBookingRepository _repository;

    public PerformerBookingController(IPerformerBookingRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PerformerBookingDto>>> GetPerformerBookings(
        CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var bookings = await _repository.ListAsync(organiserId, cancellationToken);
        return Ok(bookings.Select(ToDto));
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PerformerBookingDto>> GetPerformerBooking(
        int id, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var booking = await _repository.FindAsync(id, organiserId, cancellationToken);
        if (booking == null)
        {
            return NotFound();
        }

        return ToDto(booking);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PerformerBookingDto>> CreatePerformerBooking(
        PerformerBookingRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var booking = ToBooking(request);
        var result = await _repository.CreateAsync(booking, organiserId, cancellationToken);
        return result == PerformerBookingSaveResult.Saved
            ? CreatedAtAction(nameof(GetPerformerBooking), new { id = booking.Id }, ToDto(booking))
            : BookingError(result);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePerformerBooking(
        int id, PerformerBookingRequestDto request, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var booking = ToBooking(request);
        booking.Id = id;
        var result = await _repository.UpdateAsync(booking, organiserId, cancellationToken);
        return result == PerformerBookingSaveResult.Saved ? NoContent() : BookingError(result);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePerformerBooking(int id, CancellationToken cancellationToken)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var organiserId))
        {
            return Unauthorized();
        }

        var deleted = await _repository.DeleteAsync(id, organiserId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static PerformerBooking ToBooking(PerformerBookingRequestDto request)
    {
        return new PerformerBooking
        {
            FestivalId = request.FestivalId,
            PerformerId = request.PerformerId,
            StartsAtUtc = request.StartsAtUtc!.Value,
            EndsAtUtc = request.EndsAtUtc!.Value,
            Fee = request.Fee!.Value
        };
    }

    private ObjectResult BookingError(PerformerBookingSaveResult result)
    {
        return result switch
        {
            PerformerBookingSaveResult.BookingNotFound => Problem(
                statusCode: StatusCodes.Status404NotFound, title: "Booking not found."),
            PerformerBookingSaveResult.FestivalNotFound => Problem(
                statusCode: StatusCodes.Status404NotFound, title: "Festival not found."),
            PerformerBookingSaveResult.PerformerNotFound => Problem(
                statusCode: StatusCodes.Status404NotFound, title: "Performer not found."),
            PerformerBookingSaveResult.OutsideFestivalDates => Problem(
                statusCode: StatusCodes.Status400BadRequest, title: "The booking must be within the festival dates in UTC."),
            PerformerBookingSaveResult.OverlappingBooking => Problem(
                statusCode: StatusCodes.Status409Conflict, title: "The performer already has a booking during that time."),
            _ => throw new InvalidOperationException($"Unexpected booking result: {result}")
        };
    }

    private static PerformerBookingDto ToDto(PerformerBooking booking)
    {
        // SQLite reads DateTime values without their original kind.
        return new PerformerBookingDto(booking.Id, booking.FestivalId, booking.PerformerId,
            DateTime.SpecifyKind(booking.StartsAtUtc, DateTimeKind.Utc),
            DateTime.SpecifyKind(booking.EndsAtUtc, DateTimeKind.Utc), booking.Fee);
    }
}
