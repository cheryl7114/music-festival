namespace SOA_CA2.Models;

public class Expense
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public required string Description { get; set; }
    public decimal Amount { get; set; }
}
