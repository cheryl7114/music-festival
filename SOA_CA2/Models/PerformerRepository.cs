using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class PerformerRepository(FestivalContext context) : IPerformerRepository
{
    public async Task<List<Performer>> ListAsync(CancellationToken cancellationToken)
    {
        return await context.Performers.AsNoTracking()
            .OrderBy(performer => performer.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Performer performer, CancellationToken cancellationToken)
    {
        context.Performers.Add(performer);
        await context.SaveChangesAsync(cancellationToken);
    }
}
