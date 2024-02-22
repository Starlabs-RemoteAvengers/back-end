using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointEase.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPatientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b3d1623-aef8-4fa7-9a87-3cb7f30e6fc6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "216bd97d-af6b-4027-b81b-71cc246ed523");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "884cdf82-3a6b-46f9-aa15-57589d22bdbf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "afbeb03f-a68c-4ec2-97ea-36832d6bc729");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "TblUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "304669dd-b747-4f85-becf-95594d94414c", "3", "Doctor", "Doctor" },
                    { "3052059a-bb2d-4edd-bdaf-f35030698fe7", "2", "Clinic", "Clinic" },
                    { "30d14f8e-9a56-4201-bd70-15682eb333eb", "1", "Admin", "Admin" },
                    { "d3ec6ed6-6f76-4f7c-a34d-1123919f8fd8", "4", "Patient", "Patient" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "304669dd-b747-4f85-becf-95594d94414c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3052059a-bb2d-4edd-bdaf-f35030698fe7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30d14f8e-9a56-4201-bd70-15682eb333eb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3ec6ed6-6f76-4f7c-a34d-1123919f8fd8");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "TblUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b3d1623-aef8-4fa7-9a87-3cb7f30e6fc6", "1", "Admin", "Admin" },
                    { "216bd97d-af6b-4027-b81b-71cc246ed523", "2", "Clinic", "Clinic" },
                    { "884cdf82-3a6b-46f9-aa15-57589d22bdbf", "4", "Patient", "Patient" },
                    { "afbeb03f-a68c-4ec2-97ea-36832d6bc729", "3", "Doctor", "Doctor" }
                });
        }
    }
}
