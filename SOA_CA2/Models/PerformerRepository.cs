namespace SOA_CA2.Models;

public class PerformerRepository(FestivalContext context) : IPerformerRepository
{
    public async Task AddAsync(Performer performer, CancellationToken cancellationToken)
    {
        context.Performers.Add(performer);
        await context.SaveChangesAsync(cancellationToken);
    }
}
