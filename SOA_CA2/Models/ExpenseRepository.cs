using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class ExpenseRepository(FestivalContext context) : IExpenseRepository
{
    public async Task<List<Expense>> ListAsync(int organiserId, CancellationToken cancellationToken)
    {
        return await OwnedExpenses(organiserId).AsNoTracking()
            .OrderBy(expense => expense.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Expense?> FindAsync(int id, int organiserId, CancellationToken cancellationToken)
    {
        return await OwnedExpenses(organiserId).AsNoTracking()
            .SingleOrDefaultAsync(expense => expense.Id == id, cancellationToken);
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

    public async Task<bool> UpdateAsync(Expense expense, int organiserId, CancellationToken cancellationToken)
    {
        var affectedRows = await OwnedExpenses(organiserId)
            .Where(existing => existing.Id == expense.Id && context.Festivals.Any(festival =>
                festival.Id == expense.FestivalId && festival.OrganiserId == organiserId))
            .ExecuteUpdateAsync(update => update
                .SetProperty(existing => existing.FestivalId, expense.FestivalId)
                .SetProperty(existing => existing.Description, expense.Description)
                .SetProperty(existing => existing.Amount, expense.Amount), cancellationToken);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id, int organiserId, CancellationToken cancellationToken)
    {
        var affectedRows = await OwnedExpenses(organiserId)
            .Where(expense => expense.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows > 0;
    }

    private IQueryable<Expense> OwnedExpenses(int organiserId)
    {
        return context.Expenses.Where(expense => context.Festivals.Any(festival =>
            festival.Id == expense.FestivalId && festival.OrganiserId == organiserId));
    }
}
