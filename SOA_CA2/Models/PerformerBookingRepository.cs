using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class PerformerBookingRepository(FestivalContext context) : IPerformerBookingRepository
{
    public async Task<List<PerformerBooking>> ListAsync(int organiserId, CancellationToken cancellationToken)
    {
        return await OwnedBookings(organiserId).AsNoTracking()
            .OrderBy(booking => booking.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<PerformerBooking?> FindAsync(int id, int organiserId, CancellationToken cancellationToken)
    {
        return await OwnedBookings(organiserId).AsNoTracking()
            .SingleOrDefaultAsync(booking => booking.Id == id, cancellationToken);
    }

    private IQueryable<PerformerBooking> OwnedBookings(int organiserId)
    {
        return context.PerformerBookings.Where(booking => context.Festivals.Any(festival =>
            festival.Id == booking.FestivalId && festival.OrganiserId == organiserId));
    }

    public Task<PerformerBookingSaveResult> CreateAsync(
        PerformerBooking booking, int organiserId, CancellationToken cancellationToken)
    {
        return SaveAsync(booking, organiserId, null, cancellationToken);
    }

    public Task<PerformerBookingSaveResult> UpdateAsync(
        PerformerBooking booking, int organiserId, CancellationToken cancellationToken)
    {
        return SaveAsync(booking, organiserId, booking.Id, cancellationToken);
    }

    private async Task<PerformerBookingSaveResult> SaveAsync(
        PerformerBooking booking, int organiserId, int? bookingId, CancellationToken cancellationToken)
    {
        // SQLite's default transaction takes a write lock before the overlap check.
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        PerformerBooking? current = null;
        if (bookingId.HasValue)
        {
            current = await OwnedBookings(organiserId)
                .SingleOrDefaultAsync(existing => existing.Id == bookingId, cancellationToken);
            if (current == null)
            {
                return PerformerBookingSaveResult.BookingNotFound;
            }
        }

        var festival = await context.Festivals.AsNoTracking().SingleOrDefaultAsync(
            festival => festival.Id == booking.FestivalId && festival.OrganiserId == organiserId,
            cancellationToken);
        if (festival == null)
        {
            return PerformerBookingSaveResult.FestivalNotFound;
        }

        if (!await context.Performers.AnyAsync(performer => performer.Id == booking.PerformerId, cancellationToken))
        {
            return PerformerBookingSaveResult.PerformerNotFound;
        }

        if (DateOnly.FromDateTime(booking.StartsAtUtc) < festival.StartDate ||
            DateOnly.FromDateTime(booking.EndsAtUtc.AddTicks(-1)) > festival.EndDate)
        {
            return PerformerBookingSaveResult.OutsideFestivalDates;
        }

        var overlaps = await context.PerformerBookings.AnyAsync(existing =>
            existing.Id != bookingId &&
            existing.PerformerId == booking.PerformerId &&
            existing.StartsAtUtc < booking.EndsAtUtc &&
            existing.EndsAtUtc > booking.StartsAtUtc, cancellationToken);
        if (overlaps)
        {
            return PerformerBookingSaveResult.OverlappingBooking;
        }

        if (current == null)
        {
            context.PerformerBookings.Add(booking);
        }
        else
        {
            context.Entry(current).CurrentValues.SetValues(booking);
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return PerformerBookingSaveResult.Saved;
    }
}
