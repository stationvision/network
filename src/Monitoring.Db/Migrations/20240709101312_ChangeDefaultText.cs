using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace Monitoring.Db.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDefaultText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "25652645-29b9-479b-979b-47f488b6c328", null, "User", "USER" },
                    { "62fff456-1a5f-4a0c-a7a0-dd74f71490fb", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3555e5a0-e324-47b7-af30-bb14636239b7", 0, "9bcb4332-a7f5-4fd3-b3b0-ea924bdd67ad", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEAQYe0f6EyemCkxyoMiE4GzjDSg8ghfYvboaiXA/2DoFNUHIGRVEOu9oA5odWRqVmg==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.UpdateData(
                table: "pulseTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "دستی");

            migrationBuilder.UpdateData(
                table: "pulseTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "سخت افزاری");

            migrationBuilder.UpdateData(
                table: "pulsenatures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "وضعیت");

            migrationBuilder.UpdateData(
                table: "pulsenatures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "تعداد");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "62fff456-1a5f-4a0c-a7a0-dd74f71490fb", "3555e5a0-e324-47b7-af30-bb14636239b7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "25652645-29b9-479b-979b-47f488b6c328");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "62fff456-1a5f-4a0c-a7a0-dd74f71490fb", "3555e5a0-e324-47b7-af30-bb14636239b7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62fff456-1a5f-4a0c-a7a0-dd74f71490fb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3555e5a0-e324-47b7-af30-bb14636239b7");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7d51f91a-2744-4936-aaea-c70aeeaec3e1", null, "User", "USER" },
                    { "a41f28d3-3d94-4ede-b877-20409d5797d0", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "978356f0-b9b9-4b5a-b5bf-29b66272f2f6", 0, "d08e4ebe-0775-4caa-9da5-ca0551981dab", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEOA7J3bI8N9QMdMOYkk3TTdYIvqAtnH9Zzo8MXN8rJHgjYW4L1tVKulyBVhlXYiRww==", null, false, "", false, "admin@admin.com" });

            migrationBuilder.UpdateData(
                table: "pulseTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Manual");

            migrationBuilder.UpdateData(
                table: "pulseTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "HardwareBase");

            migrationBuilder.UpdateData(
                table: "pulsenatures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Status");

            migrationBuilder.UpdateData(
                table: "pulsenatures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Quantity");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a41f28d3-3d94-4ede-b877-20409d5797d0", "978356f0-b9b9-4b5a-b5bf-29b66272f2f6" });
        }
    }
}
