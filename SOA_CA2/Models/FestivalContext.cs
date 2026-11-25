using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class FestivalContext(DbContextOptions<FestivalContext> options)
    : DbContext(options)
{
    public DbSet<Festival> Festivals { get; set; } = null!;
    public DbSet<Organiser> Organisers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organiser>().HasIndex(organiser => organiser.Email).IsUnique();
        modelBuilder.Entity<Festival>()
            .HasOne<Organiser>()
            .WithMany()
            .HasForeignKey(festival => festival.OrganiserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
