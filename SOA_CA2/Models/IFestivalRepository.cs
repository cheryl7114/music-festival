namespace SOA_CA2.Models;

public interface IFestivalRepository
{
    Task<List<Festival>> ListAsync(int organiserId, CancellationToken cancellationToken);
    Task<Festival?> FindAsync(int id, int organiserId, CancellationToken cancellationToken);
    Task AddAsync(Festival festival, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Festival festival, int organiserId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, int organiserId, CancellationToken cancellationToken);
}
