namespace SOA_CA2.Models;

public class PerformerBooking
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public int PerformerId { get; set; }
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public decimal Fee { get; set; }
}
