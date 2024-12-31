using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Monitoring.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddingDeviationTOCLientPuls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<double>(
                name: "DeviationTime",
                table: "clientPuls",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IncreaseProductionTime",
                table: "clientPuls",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a41f28d3-3d94-4ede-b877-20409d5797d0", "978356f0-b9b9-4b5a-b5bf-29b66272f2f6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d51f91a-2744-4936-aaea-c70aeeaec3e1");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a41f28d3-3d94-4ede-b877-20409d5797d0", "978356f0-b9b9-4b5a-b5bf-29b66272f2f6" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a41f28d3-3d94-4ede-b877-20409d5797d0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "978356f0-b9b9-4b5a-b5bf-29b66272f2f6");

            migrationBuilder.DropColumn(
                name: "DeviationTime",
                table: "clientPuls");

            migrationBuilder.DropColumn(
                name: "IncreaseProductionTime",
                table: "clientPuls");

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
    }
}
