namespace SOA_CA2.Models;

public class FestivalRepository(FestivalContext context) : IFestivalRepository
{
    public async Task<Festival?> FindAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Festivals.FindAsync([id], cancellationToken);
    }

    public async Task AddAsync(Festival festival, CancellationToken cancellationToken)
    {
        context.Festivals.Add(festival);
        await context.SaveChangesAsync(cancellationToken);
    }
}
