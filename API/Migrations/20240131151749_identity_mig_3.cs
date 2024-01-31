using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class identity_mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "065f15ea-da3d-4038-9f97-d8705086cfe3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4c768ae-af70-4d0a-bb56-288f73bef29b");

            migrationBuilder.RenameColumn(
                name: "LasyName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fbcefb2-68ac-47a4-b134-f2287ce872d4", null, "Adminstrator", "ADMINSTRATOR" },
                    { "7848e6a7-6c1d-434c-a773-1db5d7b4bc65", null, "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fbcefb2-68ac-47a4-b134-f2287ce872d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7848e6a7-6c1d-434c-a773-1db5d7b4bc65");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "LasyName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "065f15ea-da3d-4038-9f97-d8705086cfe3", null, "Manager", "MANAGER" },
                    { "a4c768ae-af70-4d0a-bb56-288f73bef29b", null, "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
