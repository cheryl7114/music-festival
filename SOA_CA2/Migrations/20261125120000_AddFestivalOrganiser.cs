using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SOA_CA2.Models;

namespace SOA_CA2.Migrations;

[DbContext(typeof(FestivalContext))]
[Migration("20261125120000_AddFestivalOrganiser")]
public class AddFestivalOrganiser : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // SQLite can add the nullable foreign key inline without rebuilding Festivals.
        migrationBuilder.Sql("""
            ALTER TABLE "Festivals" ADD COLUMN "OrganiserId" INTEGER
                CONSTRAINT "FK_Festivals_Organisers_OrganiserId"
                REFERENCES "Organisers" ("Id") ON DELETE RESTRICT;
            """);

        migrationBuilder.CreateIndex(
            name: "IX_Festivals_OrganiserId",
            table: "Festivals",
            column: "OrganiserId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(name: "IX_Festivals_OrganiserId", table: "Festivals");
        migrationBuilder.Sql("""ALTER TABLE "Festivals" DROP COLUMN "OrganiserId";""");
    }
}
