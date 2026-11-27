using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class ExpenseRepository(FestivalContext context) : IExpenseRepository
{
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
