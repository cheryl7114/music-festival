using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class PerformerBookingRepository(FestivalContext context) : IPerformerBookingRepository
{
    public async Task<List<PerformerBooking>> ListAsync(int organiserId, CancellationToken cancellationToken)
    {
        return await context.PerformerBookings.AsNoTracking()
            .Where(booking => context.Festivals.Any(festival =>
                festival.Id == booking.FestivalId && festival.OrganiserId == organiserId))
            .OrderBy(booking => booking.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<PerformerBookingCreateResult> CreateAsync(
        PerformerBooking booking, int organiserId, CancellationToken cancellationToken)
    {
        // SQLite's default transaction takes a write lock before the overlap check.
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var festival = await context.Festivals.AsNoTracking().SingleOrDefaultAsync(
            festival => festival.Id == booking.FestivalId && festival.OrganiserId == organiserId,
            cancellationToken);
        if (festival == null)
        {
            return PerformerBookingCreateResult.FestivalNotFound;
        }

        if (!await context.Performers.AnyAsync(performer => performer.Id == booking.PerformerId, cancellationToken))
        {
            return PerformerBookingCreateResult.PerformerNotFound;
        }

        if (DateOnly.FromDateTime(booking.StartsAtUtc) < festival.StartDate ||
            DateOnly.FromDateTime(booking.EndsAtUtc.AddTicks(-1)) > festival.EndDate)
        {
            return PerformerBookingCreateResult.OutsideFestivalDates;
        }

        var overlaps = await context.PerformerBookings.AnyAsync(existing =>
            existing.PerformerId == booking.PerformerId &&
            existing.StartsAtUtc < booking.EndsAtUtc &&
            existing.EndsAtUtc > booking.StartsAtUtc, cancellationToken);
        if (overlaps)
        {
            return PerformerBookingCreateResult.OverlappingBooking;
        }

        context.PerformerBookings.Add(booking);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return PerformerBookingCreateResult.Created;
    }
}
