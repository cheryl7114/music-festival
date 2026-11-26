using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class FestivalRepository(FestivalContext context) : IFestivalRepository
{
    public async Task<List<Festival>> ListAsync(int organiserId, CancellationToken cancellationToken)
    {
        return await context.Festivals.AsNoTracking()
            .Where(festival => festival.OrganiserId == organiserId)
            .OrderBy(festival => festival.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Festival?> FindAsync(int id, int organiserId, CancellationToken cancellationToken)
    {
        return await context.Festivals.AsNoTracking()
            .SingleOrDefaultAsync(
                festival => festival.Id == id && festival.OrganiserId == organiserId,
                cancellationToken);
    }

    public async Task AddAsync(Festival festival, CancellationToken cancellationToken)
    {
        context.Festivals.Add(festival);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Festival festival, int organiserId, CancellationToken cancellationToken)
    {
        var affectedRows = await context.Festivals
            .Where(existing => existing.Id == festival.Id && existing.OrganiserId == organiserId)
            .ExecuteUpdateAsync(update => update
                .SetProperty(existing => existing.Name, festival.Name)
                .SetProperty(existing => existing.Location, festival.Location)
                .SetProperty(existing => existing.StartDate, festival.StartDate)
                .SetProperty(existing => existing.EndDate, festival.EndDate), cancellationToken);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, int organiserId, CancellationToken cancellationToken)
    {
        var affectedRows = await context.Festivals
            .Where(festival => festival.Id == id && festival.OrganiserId == organiserId)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows > 0;
    }
}
