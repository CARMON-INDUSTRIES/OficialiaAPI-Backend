using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OficialiaCrudAPI.Data;

#nullable disable

namespace OficialiaCrudAPI.Migrations
{
    [DbContext(typeof(AppDataDbContext))]
    [Migration("20261118180000_FechasSeguimiento")]
    public class FechasSeguimiento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FechaEnvio", table: "Correspondencia", type: "datetimeoffset", nullable: true);
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FechaCambioEstado", table: "Correspondencia", type: "datetimeoffset", nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FechaEnvio", table: "Correspondencia");
            migrationBuilder.DropColumn(name: "FechaCambioEstado", table: "Correspondencia");
        }
    }
}
