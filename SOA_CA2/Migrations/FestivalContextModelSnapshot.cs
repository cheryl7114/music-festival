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

        modelBuilder.Entity("SOA_CA2.Models.Festival", entity =>
        {
            entity.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            entity.Property<string>("Name").IsRequired().HasColumnType("TEXT");
            entity.Property<string>("Location").IsRequired().HasColumnType("TEXT");
            entity.Property<DateOnly>("StartDate").HasColumnType("TEXT");
            entity.Property<DateOnly>("EndDate").HasColumnType("TEXT");
            entity.HasKey("Id");
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
    }
}
