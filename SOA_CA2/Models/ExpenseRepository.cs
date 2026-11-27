using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class ExpenseRepository(FestivalContext context) : IExpenseRepository
{
    public async Task<List<Expense>> ListAsync(int organiserId, CancellationToken cancellationToken)
    {
        return await context.Expenses.AsNoTracking()
            .Where(expense => context.Festivals.Any(festival =>
                festival.Id == expense.FestivalId && festival.OrganiserId == organiserId))
            .OrderBy(expense => expense.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> TryAddAsync(Expense expense, int organiserId, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var ownsFestival = await context.Festivals.AnyAsync(
            festival => festival.Id == expense.FestivalId && festival.OrganiserId == organiserId,
            cancellationToken);
        if (!ownsFestival)
        {
            return false;
        }

        context.Expenses.Add(expense);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}
