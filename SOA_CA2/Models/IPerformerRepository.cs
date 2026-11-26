namespace SOA_CA2.Models;

public interface IPerformerRepository
{
    Task<List<Performer>> ListAsync(CancellationToken cancellationToken);
    Task AddAsync(Performer performer, CancellationToken cancellationToken);
}
