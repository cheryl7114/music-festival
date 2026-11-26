namespace SOA_CA2.Models;

public class Festival
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int? OrganiserId { get; set; }
}
