namespace SOA_CA2.Models;

public interface IPerformerBookingRepository
{
    Task<List<PerformerBooking>> ListAsync(int organiserId, CancellationToken cancellationToken);

    Task<PerformerBookingCreateResult> CreateAsync(
        PerformerBooking booking, int organiserId, CancellationToken cancellationToken);
}
