using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class change_statusName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "0d10cb66-b8e7-4b07-80e6-0a73da9560af");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "474d7f90-aad3-449f-a976-151d0a8edfdf");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "83d69519-2184-4544-bae2-4386d79d8436", new DateTime(2021, 4, 30, 3, 10, 2, 567, DateTimeKind.Utc).AddTicks(3443), "AQAAAAEAACcQAAAAEDNM4qI6SJsubURcTAVqFy7CsNtqZVQq2lj7pU7995mJv6OHo1iA/hc4tzHbb2NVxg==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 30, 3, 10, 2, 595, DateTimeKind.Utc).AddTicks(5650));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 30, 3, 10, 2, 594, DateTimeKind.Utc).AddTicks(6278));

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "StatusName",
                value: "Khóa hoạt động");

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "StatusName",
                value: "Hoạt động");

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "Chờ duyệt");

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "StatusName",
                value: "Ẩn");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 30, 3, 10, 2, 593, DateTimeKind.Utc).AddTicks(8381));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "5ac69faf-9363-4710-8841-73a320676d48");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "2ac91cc8-c902-4cb8-8484-63cd508ef625");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "e54973a6-62ca-4a75-9e42-caf31a6cffec", new DateTime(2021, 4, 29, 8, 4, 27, 382, DateTimeKind.Utc).AddTicks(7983), "AQAAAAEAACcQAAAAEGQyQIg88Ika6KAbX4DVDT+QBOPYeLUapMb0Y9z76myA8pvTKUKVBvIUqIpXFqTmtA==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 29, 8, 4, 27, 410, DateTimeKind.Utc).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 29, 8, 4, 27, 409, DateTimeKind.Utc).AddTicks(9413));

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "StatusName",
                value: "Khóa sản phẩm hiển thị");

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "StatusName",
                value: "Cho phép sản phẩm hiển thị");

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "Hiện sản phẩm");

            migrationBuilder.UpdateData(
                table: "ProductStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "StatusName",
                value: "Ẩn sản phẩm");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 29, 8, 4, 27, 409, DateTimeKind.Utc).AddTicks(1344));
        }
    }
}
