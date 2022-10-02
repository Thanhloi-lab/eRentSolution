using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class secgen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "4fa7e2c1-388b-405c-8fa8-b4eee5c6b1a1");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "d4aa613c-b7ba-4fbc-8b4a-4d38ebae3bae");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "7280d150-0a2e-4d65-a069-e87cbd380920", new DateTime(2022, 8, 26, 15, 4, 57, 203, DateTimeKind.Utc).AddTicks(6343), "AQAAAAEAACcQAAAAEKdYnyAZ6Olz0OExu4yUG/bYVu/Tt2iLoPIYYEFBGNZS+d77wTZKE8pGit7wikuVIw==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2022, 8, 26, 15, 4, 57, 229, DateTimeKind.Utc).AddTicks(7401));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 8, 26, 15, 4, 57, 228, DateTimeKind.Utc).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 8, 26, 15, 4, 57, 229, DateTimeKind.Utc).AddTicks(3634));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Detail",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "0662406b-9145-4a0b-8530-52ade85f6a34");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "681d57ac-da00-4b3f-bc3b-38c2b42c1451");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "c3205165-8282-4e59-96e5-66722162080c", new DateTime(2022, 8, 13, 9, 48, 4, 799, DateTimeKind.Utc).AddTicks(2624), "AQAAAAEAACcQAAAAEKejfrxhhzrjh78Y1xAhX1W2T2APENUA8voYfq5A5CHUFzPz2qXkc+tJD8G2oDJ3qA==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2022, 8, 13, 9, 48, 4, 824, DateTimeKind.Utc).AddTicks(3020));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 8, 13, 9, 48, 4, 823, DateTimeKind.Utc).AddTicks(797));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 8, 13, 9, 48, 4, 823, DateTimeKind.Utc).AddTicks(6545));
        }
    }
}
