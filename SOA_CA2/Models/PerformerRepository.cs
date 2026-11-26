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

    public async Task<Performer?> FindAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Performers.AsNoTracking()
            .SingleOrDefaultAsync(performer => performer.Id == id, cancellationToken);
    }

    public async Task AddAsync(Performer performer, CancellationToken cancellationToken)
    {
        context.Performers.Add(performer);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(int id, int organiserId, string name, CancellationToken cancellationToken)
    {
        var affectedRows = await context.Performers
            .Where(performer => performer.Id == id && performer.OrganiserId == organiserId)
            .ExecuteUpdateAsync(update => update.SetProperty(performer => performer.Name, name), cancellationToken);

        return affectedRows > 0;
    }
}
