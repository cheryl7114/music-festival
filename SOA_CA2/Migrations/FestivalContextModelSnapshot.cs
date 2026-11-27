using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SOA_CA2.Models;

namespace SOA_CA2.Migrations;

[DbContext(typeof(FestivalContext))]
public class FestivalContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.14");

        modelBuilder.Entity("SOA_CA2.Models.Expense", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<int>("FestivalId").HasColumnType("INTEGER");
            entity.Property<string>("Description").IsRequired().HasColumnType("TEXT");
            entity.Property<decimal>("Amount").HasColumnType("TEXT");
            entity.HasKey("Id");
            entity.HasIndex("FestivalId");
            entity.ToTable("Expenses");
        });

        modelBuilder.Entity("SOA_CA2.Models.PerformerBooking", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<int>("FestivalId").HasColumnType("INTEGER");
            entity.Property<int>("PerformerId").HasColumnType("INTEGER");
            entity.Property<DateTime>("StartsAtUtc").HasColumnType("TEXT");
            entity.Property<DateTime>("EndsAtUtc").HasColumnType("TEXT");
            entity.Property<decimal>("Fee").HasColumnType("TEXT");
            entity.HasKey("Id");
            entity.HasIndex("FestivalId");
            entity.HasIndex("PerformerId");
            entity.ToTable("PerformerBookings");
        });

        modelBuilder.Entity("SOA_CA2.Models.Performer", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<string>("Name").IsRequired().HasColumnType("TEXT");
            entity.Property<int>("OrganiserId").HasColumnType("INTEGER");
            entity.HasKey("Id");
            entity.HasIndex("OrganiserId");
            entity.ToTable("Performers");
        });

        modelBuilder.Entity("SOA_CA2.Models.Festival", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<string>("Name").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("Location").IsRequired().HasColumnType("TEXT");
            entity.Property<DateOnly>("StartDate").HasColumnType("TEXT");
            entity.Property<DateOnly>("EndDate").HasColumnType("TEXT");
            entity.Property<int?>("OrganiserId").HasColumnType("INTEGER");
            entity.HasKey("Id");
            entity.HasIndex("OrganiserId");
            entity.ToTable("Festivals");
        });

        modelBuilder.Entity("SOA_CA2.Models.Organiser", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<string>("Email").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("PasswordHash").IsRequired().HasColumnType("TEXT");
            entity.HasKey("Id");
            entity.HasIndex("Email").IsUnique();
            entity.ToTable("Organisers");
        });

        modelBuilder.Entity("SOA_CA2.Models.Expense", entity =>
        {
            entity.HasOne("SOA_CA2.Models.Festival", null)
                .WithMany()
                .HasForeignKey("FestivalId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("SOA_CA2.Models.PerformerBooking", entity =>
        {
            entity.HasOne("SOA_CA2.Models.Festival", null)
                .WithMany()
                .HasForeignKey("FestivalId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasOne("SOA_CA2.Models.Performer", null)
                .WithMany()
                .HasForeignKey("PerformerId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });

        modelBuilder.Entity("SOA_CA2.Models.Festival", entity =>
        {
            entity.HasOne("SOA_CA2.Models.Organiser", null)
                .WithMany()
                .HasForeignKey("OrganiserId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity("SOA_CA2.Models.Performer", entity =>
        {
            entity.HasOne("SOA_CA2.Models.Organiser", null)
                .WithMany()
                .HasForeignKey("OrganiserId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });
    }
}
