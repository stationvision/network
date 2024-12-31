using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Monitoring.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddingMessageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "clientPuls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d26c7aae-274f-48ba-8bcf-ab988b64e015", null, "User", "USER" },
                    { "db3b8445-8f49-4daf-b2f3-6d8eb02688a3", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e49efeea-4cf6-4d1e-ac1b-d561e1f9d0bb", 0, "198a9886-47d5-48e9-ae1e-ebcdab2c18b1", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEFxr7M3VECBDNKLfJ0N9y5QPr/pF4HMhlFMSHQH0eTe0Hksi0YL4Alh/k4Si/FrhxQ==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "db3b8445-8f49-4daf-b2f3-6d8eb02688a3", "e49efeea-4cf6-4d1e-ac1b-d561e1f9d0bb" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d26c7aae-274f-48ba-8bcf-ab988b64e015");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "db3b8445-8f49-4daf-b2f3-6d8eb02688a3", "e49efeea-4cf6-4d1e-ac1b-d561e1f9d0bb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db3b8445-8f49-4daf-b2f3-6d8eb02688a3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e49efeea-4cf6-4d1e-ac1b-d561e1f9d0bb");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "clientPuls");

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
    }
}
