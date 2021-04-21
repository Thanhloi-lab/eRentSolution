using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class user_and_category_image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Products",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Categories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ImageSize",
                table: "Categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "AvatarFilePath",
                table: "AppUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AvatarFileSize",
                table: "AppUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "a80a03c2-86a5-4e2f-b601-045d5c4ff3ea");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "e230ebe6-793c-4cf9-a7f6-747f1d70aae0");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "02d8b747-afc1-4af0-8a10-674cd6059b35", new DateTime(2021, 4, 21, 4, 56, 28, 339, DateTimeKind.Utc).AddTicks(8502), "AQAAAAEAACcQAAAAEPguN9468AS5s10jQ0l7bpdVAIoUuJkloVzZN8neGqLu5KeOx+YB68ZulGTDh3Egpw==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 21, 4, 56, 28, 378, DateTimeKind.Utc).AddTicks(6586));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 21, 4, 56, 28, 377, DateTimeKind.Utc).AddTicks(7602));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "DateCreated" },
                values: new object[] { "TP.HCM-Hóc Môn-Xã Tân Thới Nhì-Ấp Dân Thắng 1, 77/3", new DateTime(2021, 4, 21, 4, 56, 28, 376, DateTimeKind.Utc).AddTicks(7775) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageSize",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AvatarFilePath",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "AvatarFileSize",
                table: "AppUsers");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "a776acd6-1c47-47be-9896-1a619013ac53");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "a73dde6f-8098-4d70-b84d-977511c231a6");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "30632708-21a4-4ba1-8070-51f5ac3b655d", new DateTime(2021, 4, 15, 4, 14, 28, 12, DateTimeKind.Utc).AddTicks(9220), "AQAAAAEAACcQAAAAEN3QP2cKkH6sdqyevwhrgref5qB/oJ4m85/qcTR8wJrXYpQdVV0DTrGdEmQwtC7kwg==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 15, 4, 14, 28, 40, DateTimeKind.Utc).AddTicks(3872));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 15, 4, 14, 28, 39, DateTimeKind.Utc).AddTicks(5790));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 15, 4, 14, 28, 38, DateTimeKind.Utc).AddTicks(7661));
        }
    }
}
