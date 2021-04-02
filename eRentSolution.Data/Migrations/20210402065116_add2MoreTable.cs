using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class add2MoreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "a6f97d12-2f00-4dee-af59-ee5deabad987", "Admin" });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("e4df483b-524d-467b-b6f4-2ee002742987"), "5a6d6cac-eb02-4cf6-bfe5-9d7dfc1b76b3", "User admin role", "UserAdmin", "useradmin" });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "75dd60e3-e152-4d05-8d01-c5f78ddfeed0", "admin" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5b2232db-febe-434f-9284-ef0fb7bf8321", "AQAAAAEAACcQAAAAEJmVaTauOF460WKqOpH5xkrtUMk/XC5whLfhC/fUsQNyih6vPQ9TxmNK5ID1TVCmFA==" });

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 2, 11, 49, 26, 807, DateTimeKind.Local).AddTicks(3217));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 2, 11, 49, 26, 804, DateTimeKind.Local).AddTicks(4156));
        }
    }
}
