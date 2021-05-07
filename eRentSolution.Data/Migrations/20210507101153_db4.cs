using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class db4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "8365e96e-cb2e-4e1e-8120-8b568149682c");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "f38a9c6a-722a-4494-9981-2da0202a391f");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "e28b00f1-32e1-42f8-aa6f-e9f5fbb68108", new DateTime(2021, 5, 7, 10, 11, 52, 162, DateTimeKind.Utc).AddTicks(3121), "AQAAAAEAACcQAAAAEE0ExTG/cMK41bl+rcfnuvhVyB/H6Lwxu8PErcql9pEYQfbWguGjVrYkV7FZGidL9g==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 5, 7, 10, 11, 52, 190, DateTimeKind.Utc).AddTicks(5748));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 11, 52, 189, DateTimeKind.Utc).AddTicks(6234));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 11, 52, 188, DateTimeKind.Utc).AddTicks(7640));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "f81b7ba3-5521-4fac-af50-2cc11e3e097d");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "eadc577d-0485-4ecf-8ec4-73f569f255d8");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "9c39aa1b-49c1-4051-91cf-685b2d423a44", new DateTime(2021, 5, 7, 10, 4, 16, 333, DateTimeKind.Utc).AddTicks(1209), "AQAAAAEAACcQAAAAEOu/oJPNSnOotw8quLNiLA2VX/h3oT9otBij6prD7GnG40MEbroC8rsbbJ7eELrcbg==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 5, 7, 10, 4, 16, 369, DateTimeKind.Utc).AddTicks(2116));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 4, 16, 368, DateTimeKind.Utc).AddTicks(2695));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 4, 16, 367, DateTimeKind.Utc).AddTicks(4164));
        }
    }
}
