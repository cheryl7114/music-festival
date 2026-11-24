using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class OrganiserRepository(FestivalContext context) : IOrganiserRepository
{
    private const int SqliteUniqueConstraint = 2067;

    public async Task<bool> TryAddAsync(Organiser organiser, CancellationToken cancellationToken)
    {
        context.Organisers.Add(organiser);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException exception) when (
            exception.InnerException is SqliteException { SqliteExtendedErrorCode: SqliteUniqueConstraint })
        {
            context.Entry(organiser).State = EntityState.Detached;
            return false;
        }
    }
}
