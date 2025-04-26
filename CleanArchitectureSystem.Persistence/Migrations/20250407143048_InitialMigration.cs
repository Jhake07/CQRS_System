using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 100, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrenceStamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BatchSerials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchQty = table.Column<int>(type: "int", nullable: false),
                    OrderQty = table.Column<int>(type: "int", nullable: false),
                    DeliverQty = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartSNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndSNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Item_ModelCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchSerials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchSerials_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MainSerials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScanTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchSerialId = table.Column<int>(type: "int", nullable: true),
                    BatchSerial_ContractNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainSerials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainSerials_BatchSerials_BatchSerialId",
                        column: x => x.BatchSerialId,
                        principalTable: "BatchSerials",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "ConcurrenceStamp", "CreatedDate", "Email", "FirstName", "IsActive", "LastLogin", "LastName", "ModifiedDate", "PasswordHash", "SecurityStamp", "Username" },
                values: new object[] { 1, "tesdasdsada", new DateTime(2025, 4, 7, 22, 30, 48, 258, DateTimeKind.Local).AddTicks(8536), "test.gmail.com", "Jake", true, new DateTime(2025, 4, 7, 22, 30, 48, 259, DateTimeKind.Local).AddTicks(9278), "Umali", new DateTime(2025, 4, 7, 22, 30, 48, 259, DateTimeKind.Local).AddTicks(8998), "12345", "testtss", "test" });

            migrationBuilder.CreateIndex(
                name: "IX_BatchSerials_ItemId",
                table: "BatchSerials",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MainSerials_BatchSerialId",
                table: "MainSerials",
                column: "BatchSerialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "MainSerials");

            migrationBuilder.DropTable(
                name: "BatchSerials");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
