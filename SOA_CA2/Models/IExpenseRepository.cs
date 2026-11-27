namespace SOA_CA2.Models;

public interface IExpenseRepository
{
    Task<bool> TryAddAsync(Expense expense, int organiserId, CancellationToken cancellationToken);
}
