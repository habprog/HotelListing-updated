using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.API.Migrations
{
    public partial class AddedDefaultRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "eda4989f-06a3-4dc8-a073-26b97f1280e3", "fc19efb7-62ec-48a3-a4c0-d1c05a2eb8d5", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c78dc51-c8e5-465a-9a56-3418b6d40962", "ebd5697b-4c2b-4a87-b752-f40f757815e2", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c78dc51-c8e5-465a-9a56-3418b6d40962");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eda4989f-06a3-4dc8-a073-26b97f1280e3");
        }
    }
}
