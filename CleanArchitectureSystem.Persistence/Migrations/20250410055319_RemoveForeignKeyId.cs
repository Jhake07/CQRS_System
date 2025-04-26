using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForeignKeyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchSerials_Items_ItemId",
                table: "BatchSerials");

            migrationBuilder.DropForeignKey(
                name: "FK_MainSerials_BatchSerials_BatchSerialId",
                table: "MainSerials");

            migrationBuilder.DropIndex(
                name: "IX_MainSerials_BatchSerialId",
                table: "MainSerials");

            migrationBuilder.DropIndex(
                name: "IX_BatchSerials_ItemId",
                table: "BatchSerials");

            migrationBuilder.DropColumn(
                name: "BatchSerialId",
                table: "MainSerials");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "BatchSerials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchSerialId",
                table: "MainSerials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "BatchSerials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MainSerials_BatchSerialId",
                table: "MainSerials",
                column: "BatchSerialId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchSerials_ItemId",
                table: "BatchSerials",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchSerials_Items_ItemId",
                table: "BatchSerials",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MainSerials_BatchSerials_BatchSerialId",
                table: "MainSerials",
                column: "BatchSerialId",
                principalTable: "BatchSerials",
                principalColumn: "Id");
        }
    }
}
