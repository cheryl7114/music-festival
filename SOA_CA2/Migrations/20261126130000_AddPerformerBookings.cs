using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SOA_CA2.Models;

namespace SOA_CA2.Migrations;

[DbContext(typeof(FestivalContext))]
[Migration("20261126130000_AddPerformerBookings")]
public class AddPerformerBookings : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PerformerBookings",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                FestivalId = table.Column<int>(type: "INTEGER", nullable: false),
                PerformerId = table.Column<int>(type: "INTEGER", nullable: false),
                StartsAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                EndsAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                Fee = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PerformerBookings", booking => booking.Id);
                table.ForeignKey(
                    name: "FK_PerformerBookings_Festivals_FestivalId",
                    column: booking => booking.FestivalId,
                    principalTable: "Festivals",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PerformerBookings_Performers_PerformerId",
                    column: booking => booking.PerformerId,
                    principalTable: "Performers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PerformerBookings_FestivalId",
            table: "PerformerBookings",
            column: "FestivalId");
        migrationBuilder.CreateIndex(
            name: "IX_PerformerBookings_PerformerId",
            table: "PerformerBookings",
            column: "PerformerId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "PerformerBookings");
    }
}
