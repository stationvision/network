using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Monitoring.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddingStatusToClientPuls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "clientPuls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "108a0946-5c2c-4fa2-af84-38ecec21fea5", null, "User", "USER" },
                    { "4a1e7dac-2fcc-40e4-a173-f563f58be463", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "db895c60-f585-4ed9-88f4-7da7e15c9645", 0, "2c5ae585-960a-46bf-8edf-4fc6c2519a7e", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEHYfRP+rBu0VVebQA5C8CETP6xet6UxkflZ2aWy+Cz81QGiauUHMWj7awF7gBejNug==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4a1e7dac-2fcc-40e4-a173-f563f58be463", "db895c60-f585-4ed9-88f4-7da7e15c9645" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "108a0946-5c2c-4fa2-af84-38ecec21fea5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4a1e7dac-2fcc-40e4-a173-f563f58be463", "db895c60-f585-4ed9-88f4-7da7e15c9645" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a1e7dac-2fcc-40e4-a173-f563f58be463");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "db895c60-f585-4ed9-88f4-7da7e15c9645");

            migrationBuilder.DropColumn(
                name: "status",
                table: "clientPuls");

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
        }
    }
}
