using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropAppUserTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "AppUser");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
