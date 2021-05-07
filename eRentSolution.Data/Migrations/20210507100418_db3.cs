using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class db3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 4,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 4);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "3aa758cb-03b9-4a16-b4f2-d425df0479cd");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "6a2ffc83-ef2a-4baf-9995-7026b6d5ff70");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "4bc40c09-8671-4e81-9300-0e9671a19bc1", new DateTime(2021, 5, 7, 10, 1, 2, 780, DateTimeKind.Utc).AddTicks(9258), "AQAAAAEAACcQAAAAED5u0iRkLbAPWs8ANblSYwqrFB2+eoCqtOleqI5eISlO9iTAHTH1WEodS90B95ZH5g==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 5, 7, 10, 1, 2, 810, DateTimeKind.Utc).AddTicks(6018));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 1, 2, 809, DateTimeKind.Utc).AddTicks(5746));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 1, 2, 808, DateTimeKind.Utc).AddTicks(6337));
        }
    }
}
