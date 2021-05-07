using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class db5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "8235eb62-82a7-4afc-bf77-0d65abd06048");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "34e98e13-85b9-4d88-a64f-d507b6db24b3");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "a3f9a981-1fad-4856-aada-f1813ef47b8f", new DateTime(2021, 5, 7, 10, 43, 55, 652, DateTimeKind.Utc).AddTicks(3871), "AQAAAAEAACcQAAAAEAE8n0OAMDav9Bt6mv6PAmQZmcyrZkUjlapnOLh25pTjOlwfnuczeff7qHikO+qiSg==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 5, 7, 10, 43, 55, 680, DateTimeKind.Utc).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 43, 55, 679, DateTimeKind.Utc).AddTicks(9076));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 43, 55, 678, DateTimeKind.Utc).AddTicks(9921));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplyForAll = table.Column<bool>(type: "bit", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountPercent = table.Column<int>(type: "int", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCategoryIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

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
    }
}
