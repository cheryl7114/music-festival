using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SOA_CA2.Models;

namespace SOA_CA2.Migrations;

[DbContext(typeof(FestivalContext))]
[Migration("20261124180000_AddOrganisers")]
public class AddOrganisers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Existing local databases already have this table from EnsureCreated.
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS "Festivals" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_Festivals" PRIMARY KEY AUTOINCREMENT,
                "Name" TEXT NOT NULL,
                "Location" TEXT NOT NULL,
                "StartDate" TEXT NOT NULL,
                "EndDate" TEXT NOT NULL
            );
            """);

        migrationBuilder.CreateTable(
            name: "Organisers",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Email = table.Column<string>(type: "TEXT", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Organisers", organiser => organiser.Id));

        migrationBuilder.CreateIndex(
            name: "IX_Organisers_Email",
            table: "Organisers",
            column: "Email",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Organisers");
    }
}
