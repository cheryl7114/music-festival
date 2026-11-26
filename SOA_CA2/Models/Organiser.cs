namespace SOA_CA2.Models;

public class Organiser
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}
