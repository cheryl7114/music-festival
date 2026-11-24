namespace SOA_CA2.Models;

public interface IFestivalRepository
{
    Task<List<Festival>> ListAsync(CancellationToken cancellationToken);
    Task<Festival?> FindAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Festival festival, CancellationToken cancellationToken);
}
