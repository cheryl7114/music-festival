using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class FestivalContext(DbContextOptions<FestivalContext> options)
    : DbContext(options)
{
    public DbSet<Festival> Festivals { get; set; } = null!;
}
