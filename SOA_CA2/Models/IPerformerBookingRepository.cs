namespace SOA_CA2.Models;

public interface IPerformerBookingRepository
{
    Task<List<PerformerBooking>> ListAsync(int organiserId, CancellationToken cancellationToken);
    Task<PerformerBooking?> FindAsync(int id, int organiserId, CancellationToken cancellationToken);

    Task<PerformerBookingSaveResult> CreateAsync(
        PerformerBooking booking, int organiserId, CancellationToken cancellationToken);

    Task<PerformerBookingSaveResult> UpdateAsync(
        PerformerBooking booking, int organiserId, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, int organiserId, CancellationToken cancellationToken);
}
