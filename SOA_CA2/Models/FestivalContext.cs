using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models;

public class FestivalContext(DbContextOptions<FestivalContext> options)
    : DbContext(options)
{
    public DbSet<Festival> Festivals { get; set; } = null!;
    public DbSet<Organiser> Organisers { get; set; } = null!;
    public DbSet<Performer> Performers { get; set; } = null!;
    public DbSet<PerformerBooking> PerformerBookings { get; set; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organiser>().HasIndex(organiser => organiser.Email).IsUnique();
        modelBuilder.Entity<Festival>()
            .HasOne<Organiser>()
            .WithMany()
            .HasForeignKey(festival => festival.OrganiserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Performer>()
            .HasOne<Organiser>()
            .WithMany()
            .HasForeignKey(performer => performer.OrganiserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PerformerBooking>()
            .HasOne<Festival>()
            .WithMany()
            .HasForeignKey(booking => booking.FestivalId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PerformerBooking>()
            .HasOne<Performer>()
            .WithMany()
            .HasForeignKey(booking => booking.PerformerId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Expense>()
            .HasOne<Festival>()
            .WithMany()
            .HasForeignKey(expense => expense.FestivalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
