namespace SOA_CA2.Models;

public interface IExpenseRepository
{
    Task<List<Expense>> ListAsync(int organiserId, CancellationToken cancellationToken);
    Task<Expense?> FindAsync(int id, int organiserId, CancellationToken cancellationToken);
    Task<bool> TryAddAsync(Expense expense, int organiserId, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Expense expense, int organiserId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, int organiserId, CancellationToken cancellationToken);
}
