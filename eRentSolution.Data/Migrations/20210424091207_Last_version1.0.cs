using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class Last_version10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "08a94b04-540e-4a19-92c6-2519f486a2de");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "6db6b96b-d4be-4a28-9dc8-0f7252217c72");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "d1a0a6a4-02d6-4bf0-8730-8cfc0b59fd43", new DateTime(2021, 4, 24, 9, 12, 6, 322, DateTimeKind.Utc).AddTicks(3538), "AQAAAAEAACcQAAAAENXCG0gL/n0YsPYdpe6Pn4WSSoDFIPjfvA8adQFht99lvRbiducpbL54sp31yWyTpw==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 24, 9, 12, 6, 352, DateTimeKind.Utc).AddTicks(2211));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 24, 9, 12, 6, 351, DateTimeKind.Utc).AddTicks(2897));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 24, 9, 12, 6, 350, DateTimeKind.Utc).AddTicks(4943));

            migrationBuilder.InsertData(
                table: "UserActions",
                columns: new[] { "Id", "ActionName" },
                values: new object[,]
                {
                    { 16, "Chỉnh sửa hình ảnh sản phẩm" },
                    { 17, "Thêm hình ảnh sản phẩm" },
                    { 18, "Xóa hình ảnh sản phẩm" },
                    { 19, "Xóa chi tiết sản phẩm" },
                    { 20, "Thêm chi tiết sản phẩm" },
                    { 21, "Chỉnh sửa chi tiết sản phẩm" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "28ec0373-fc2c-41ff-97fa-f0039253b6d6");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "f49ec332-12b4-42a6-aa0d-fa5ab4f6bc2b");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "a38c4112-7c63-43f5-9087-1207dcf4c05c", new DateTime(2021, 4, 22, 3, 56, 16, 759, DateTimeKind.Utc).AddTicks(6760), "AQAAAAEAACcQAAAAEIxZWQ/SzsorE1vazA78mpP2czSzAepIV2Fhje5G47HrSRqkanveXMaEDZVdnaQR4Q==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 22, 3, 56, 16, 788, DateTimeKind.Utc).AddTicks(5547));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 22, 3, 56, 16, 787, DateTimeKind.Utc).AddTicks(5876));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 22, 3, 56, 16, 786, DateTimeKind.Utc).AddTicks(7579));
        }
    }
}
