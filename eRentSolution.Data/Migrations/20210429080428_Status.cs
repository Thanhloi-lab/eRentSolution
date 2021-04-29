using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStatus_StatusId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStatus",
                table: "ProductStatus");

            migrationBuilder.RenameTable(
                name: "ProductStatus",
                newName: "ProductStatuses");

            migrationBuilder.AlterColumn<string>(
                name: "SeoAlias",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "ProductStatuses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses",
                column: "Id");

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
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 29, 8, 4, 27, 409, DateTimeKind.Utc).AddTicks(1344));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStatuses_StatusId",
                table: "Products",
                column: "StatusId",
                principalTable: "ProductStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStatuses_StatusId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStatuses",
                table: "ProductStatuses");

            migrationBuilder.RenameTable(
                name: "ProductStatuses",
                newName: "ProductStatus");

            migrationBuilder.AlterColumn<string>(
                name: "SeoAlias",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "ProductStatus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStatus",
                table: "ProductStatus",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "3600e703-381f-4115-814d-d39c730222b8");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "a259e722-4968-4785-a048-f77f2e954c7d");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "34d088bc-ecb8-4561-9f16-f954ef705a08", new DateTime(2021, 4, 28, 12, 53, 41, 614, DateTimeKind.Utc).AddTicks(4260), "AQAAAAEAACcQAAAAEKeYUu52yn8LKPfosC5Myjy2ri8NdOol/mGxo6Y4fkZJUDX3Amhmby3VxjEqHn31XQ==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 28, 12, 53, 41, 644, DateTimeKind.Utc).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 28, 12, 53, 41, 643, DateTimeKind.Utc).AddTicks(6487));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 28, 12, 53, 41, 642, DateTimeKind.Utc).AddTicks(7033));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStatus_StatusId",
                table: "Products",
                column: "StatusId",
                principalTable: "ProductStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
