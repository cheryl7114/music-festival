namespace SOA_CA2.Models;

public interface IExpenseRepository
{
    Task<List<Expense>> ListAsync(int organiserId, CancellationToken cancellationToken);
    Task<bool> TryAddAsync(Expense expense, int organiserId, CancellationToken cancellationToken);
}
