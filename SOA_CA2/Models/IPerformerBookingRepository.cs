namespace SOA_CA2.Models;

public interface IPerformerBookingRepository
{
    Task<PerformerBookingCreateResult> CreateAsync(
        PerformerBooking booking, int organiserId, CancellationToken cancellationToken);
}
