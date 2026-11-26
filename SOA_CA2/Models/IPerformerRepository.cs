namespace SOA_CA2.Models;

public interface IPerformerRepository
{
    Task AddAsync(Performer performer, CancellationToken cancellationToken);
}
