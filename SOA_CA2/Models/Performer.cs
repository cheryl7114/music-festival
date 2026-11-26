namespace SOA_CA2.Models;

public class Performer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int OrganiserId { get; set; }
}
