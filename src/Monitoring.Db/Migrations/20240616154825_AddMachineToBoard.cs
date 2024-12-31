using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Monitoring.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineToBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientPuls_machines_PulsId",
                table: "clientPuls");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ba83a6f-64c2-40c6-889e-1538efc7a995");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c44e46a3-6ca9-4b63-b9a6-34dbac1d5e56", "98d8bc1f-2a41-4ccc-9b76-eacc8fd5745d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c44e46a3-6ca9-4b63-b9a6-34dbac1d5e56");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98d8bc1f-2a41-4ccc-9b76-eacc8fd5745d");

            migrationBuilder.DropColumn(
                name: "pulsId",
                table: "machines");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "clientPuls");

            migrationBuilder.AddColumn<string>(
                name: "BoardId",
                table: "machines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4068b6f3-53b6-43e9-8b3c-0f9347b4df9c", null, "Admin", "ADMIN" },
                    { "b6c6712a-0fc9-49e2-a0e1-518f8b64b123", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "02a4822b-5280-4295-b07f-a28ebc0ffff2", 0, "614ab367-f084-4271-9f12-95a18557d934", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEG2NwldexC+IDSMDyRzz0/ZLbP5NVVGILNRnZjpiZjdgc8aX8d1Hr7yFDRU4KJG19w==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4068b6f3-53b6-43e9-8b3c-0f9347b4df9c", "02a4822b-5280-4295-b07f-a28ebc0ffff2" });

            migrationBuilder.CreateIndex(
                name: "IX_machines_BoardId",
                table: "machines",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_machines_boards_BoardId",
                table: "machines",
                column: "BoardId",
                principalTable: "boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_machines_boards_BoardId",
                table: "machines");

            migrationBuilder.DropIndex(
                name: "IX_machines_BoardId",
                table: "machines");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6c6712a-0fc9-49e2-a0e1-518f8b64b123");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4068b6f3-53b6-43e9-8b3c-0f9347b4df9c", "02a4822b-5280-4295-b07f-a28ebc0ffff2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4068b6f3-53b6-43e9-8b3c-0f9347b4df9c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02a4822b-5280-4295-b07f-a28ebc0ffff2");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "machines");

            migrationBuilder.AddColumn<int>(
                name: "pulsId",
                table: "machines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "clientPuls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ba83a6f-64c2-40c6-889e-1538efc7a995", null, "User", "USER" },
                    { "c44e46a3-6ca9-4b63-b9a6-34dbac1d5e56", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "98d8bc1f-2a41-4ccc-9b76-eacc8fd5745d", 0, "3705dbe0-b23d-4165-a59f-25c82c17207f", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEL0/VzjzSkRNs9Rc1mO1bFySca9HzYTJYHhEYX3VzQlD5yVLgDoq5Q/08+7BNYjQlw==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c44e46a3-6ca9-4b63-b9a6-34dbac1d5e56", "98d8bc1f-2a41-4ccc-9b76-eacc8fd5745d" });

            migrationBuilder.AddForeignKey(
                name: "FK_clientPuls_machines_PulsId",
                table: "clientPuls",
                column: "PulsId",
                principalTable: "machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
