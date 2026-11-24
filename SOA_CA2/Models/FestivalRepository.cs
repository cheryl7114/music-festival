using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class FestivalRepository(FestivalContext context) : IFestivalRepository
{
    public async Task<List<Festival>> ListAsync(CancellationToken cancellationToken)
    {
        return await context.Festivals.AsNoTracking().OrderBy(festival => festival.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Festival?> FindAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Festivals.FindAsync([id], cancellationToken);
    }

    public async Task AddAsync(Festival festival, CancellationToken cancellationToken)
    {
        context.Festivals.Add(festival);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Festival festival, CancellationToken cancellationToken)
    {
        var affectedRows = await context.Festivals.Where(existing => existing.Id == festival.Id)
            .ExecuteUpdateAsync(update => update
                .SetProperty(existing => existing.Name, festival.Name)
                .SetProperty(existing => existing.Location, festival.Location)
                .SetProperty(existing => existing.StartDate, festival.StartDate)
                .SetProperty(existing => existing.EndDate, festival.EndDate), cancellationToken);

        return affectedRows > 0;
    }
}
