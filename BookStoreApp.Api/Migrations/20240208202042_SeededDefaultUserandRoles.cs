using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.Api.Migrations
{
    public partial class SeededDefaultUserandRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1cf7672a-1d45-4eda-8eab-b3c6ee1f6767", "3adcfa43-53f6-46d1-802f-3437e3b7e9bc", "User", "USER" },
                    { "bbd45a0d-f600-4343-b039-302564c70e1a", "995b5118-6b13-40eb-a427-fb40b735b2c3", "Administrator", "ADMINISTRATOR" }
                });

            // Maybe somethig is wrong here ?? I think I should swap 041 line with 10f line
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4c71a100-e945-419f-ba73-77f5bce0a041", 0, "1ddba253-96ad-4088-93c4-0766c8d4554c", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEL91xejc5DQ2sYPYO7uHUvg4MrPGnnwe6khTgf6CEeJuT9lyYWRSs8gl3H+2+swSxg==", null, false, "b183b8ff-c8f2-4ba7-99d6-424471d7fba5", false, "admin@bookstore.com" },
                    { "d5f31ae7-bf33-44fe-8ce4-9285a3ea410f", 0, "79e989c9-9053-46b2-833b-f756f4ff363b", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEBZrxWEZy8zr47kcjFqVJSqUyqvTP4RtHcI0RlaSlN79OXlGZgQQ8Up/lywTSCdeuA==", null, false, "972f8a81-5316-4465-a52c-3db2e15a4bd5", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1cf7672a-1d45-4eda-8eab-b3c6ee1f6767", "4c71a100-e945-419f-ba73-77f5bce0a041" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "bbd45a0d-f600-4343-b039-302564c70e1a", "4c71a100-e945-419f-ba73-77f5bce0a041" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1cf7672a-1d45-4eda-8eab-b3c6ee1f6767", "4c71a100-e945-419f-ba73-77f5bce0a041" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bbd45a0d-f600-4343-b039-302564c70e1a", "4c71a100-e945-419f-ba73-77f5bce0a041" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d5f31ae7-bf33-44fe-8ce4-9285a3ea410f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1cf7672a-1d45-4eda-8eab-b3c6ee1f6767");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbd45a0d-f600-4343-b039-302564c70e1a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4c71a100-e945-419f-ba73-77f5bce0a041");
        }
    }
}
