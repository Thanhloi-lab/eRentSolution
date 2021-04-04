using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class fixConstraintOfImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductImages",
                newName: "ProductDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                newName: "IX_ProductImages_ProductDetailId");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "939cd2b0-5316-4f96-bc32-d31065c9334c");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "3d6b62a0-7db8-4bd6-a70c-31f83dc217c0");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e6282284-0535-4141-8c0c-e18baf47e49e", "AQAAAAEAACcQAAAAEHPCp9DBoyeMb6foiDnfJgcdI6ChqH+3pJDVneLbKfjLUaXjwyW1rORgvIZRYocbhg==" });

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 3, 17, 37, 33, 236, DateTimeKind.Local).AddTicks(1605));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 3, 17, 37, 33, 234, DateTimeKind.Local).AddTicks(4652));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductDetails_ProductDetailId",
                table: "ProductImages",
                column: "ProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductDetails_ProductDetailId",
                table: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "ProductDetailId",
                table: "ProductImages",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductDetailId",
                table: "ProductImages",
                newName: "IX_ProductImages_ProductId");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "a6f97d12-2f00-4dee-af59-ee5deabad987");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "5a6d6cac-eb02-4cf6-bfe5-9d7dfc1b76b3");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "43061174-ff3b-4d66-b886-e85f0eb5aa64", "AQAAAAEAACcQAAAAEFVJ3n3vL1uULzeElFUk5/sx9tVRS+pk8i/a+XU7rYjLcmGIa3pp2Ol2+/2D17SSsg==" });

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 2, 13, 51, 15, 296, DateTimeKind.Local).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 2, 13, 51, 15, 294, DateTimeKind.Local).AddTicks(1254));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
