using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointEase.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdClinciToIdClinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "IdClinci",
                table: "TblDoctor",
                newName: "IdClinic");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2188b36b-93ec-43ae-a10d-ac734a59c3bd", "2", "Clinic", "Clinic" },
                    { "28d31ccc-765e-4136-ba11-d0c11b3a0e95", "4", "Patient", "Patient" },
                    { "328e7c80-bc7c-4aae-97f8-ef27438edf1d", "1", "Admin", "Admin" },
                    { "dde64cf4-8351-48d2-8da6-85767b3ab876", "3", "Doctor", "Doctor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2188b36b-93ec-43ae-a10d-ac734a59c3bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28d31ccc-765e-4136-ba11-d0c11b3a0e95");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "328e7c80-bc7c-4aae-97f8-ef27438edf1d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dde64cf4-8351-48d2-8da6-85767b3ab876");

            migrationBuilder.RenameColumn(
                name: "IdClinic",
                table: "TblDoctor",
                newName: "IdClinci");

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
    }
}
