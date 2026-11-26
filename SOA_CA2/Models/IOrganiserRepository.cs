namespace SOA_CA2.Models;

public interface IOrganiserRepository
{
    Task<bool> TryAddAsync(Organiser organiser, CancellationToken cancellationToken);
    Task<Organiser?> FindByEmailAsync(string email, CancellationToken cancellationToken);
}
