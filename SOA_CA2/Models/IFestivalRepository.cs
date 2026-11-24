namespace SOA_CA2.Models;

public interface IFestivalRepository
{
    Task<Festival?> FindAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Festival festival, CancellationToken cancellationToken);
}
