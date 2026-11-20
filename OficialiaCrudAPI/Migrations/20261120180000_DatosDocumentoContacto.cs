using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OficialiaCrudAPI.Data;

#nullable disable

namespace OficialiaCrudAPI.Migrations
{
    [DbContext(typeof(AppDataDbContext))]
    [Migration("20261120180000_DatosDocumentoContacto")]
    public class DatosDocumentoContacto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("TipoDocumento", "Correspondencia", type: "nvarchar(100)", maxLength: 100, nullable: true);
            migrationBuilder.AddColumn<string>("Calle", "Correspondencia", type: "nvarchar(250)", maxLength: 250, nullable: true);
            migrationBuilder.AddColumn<string>("Telefono", "Correspondencia", type: "nvarchar(50)", maxLength: 50, nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("TipoDocumento", "Correspondencia");
            migrationBuilder.DropColumn("Calle", "Correspondencia");
            migrationBuilder.DropColumn("Telefono", "Correspondencia");
        }
    }
}
