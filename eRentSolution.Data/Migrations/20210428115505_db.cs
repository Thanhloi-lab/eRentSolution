using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShowOnHome",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Categories");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "b476622f-ae5a-4478-ae23-4bc7bf61dd0c");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "a0671b6b-f274-47e8-bb62-11c3ba0eafe7");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "6e01df5e-2ba7-440d-a736-994a052cc6ab", new DateTime(2021, 4, 28, 11, 55, 4, 18, DateTimeKind.Utc).AddTicks(5209), "AQAAAAEAACcQAAAAEPp58IhTkQ9mzl/y4PDofF4mf1d2mhXrzyxNRlw1xhdSGUEeswI6mw0Uq+wMU3zuaA==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 28, 11, 55, 4, 47, DateTimeKind.Utc).AddTicks(9183));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 28, 11, 55, 4, 46, DateTimeKind.Utc).AddTicks(9309));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 28, 11, 55, 4, 46, DateTimeKind.Utc).AddTicks(326));

            migrationBuilder.InsertData(
                table: "UserActions",
                columns: new[] { "Id", "ActionName" },
                values: new object[,]
                {
                    { 22, "Không cho phép hiển thị" },
                    { 23, "Cho phép hiển thị" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "UserActions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Categories");

            migrationBuilder.AddColumn<bool>(
                name: "IsShowOnHome",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "7f24030b-3951-4cc0-a07f-b8d8b9870bf1");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "a635158d-e816-4f1d-b751-363d9d0dc64a");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "b7166cda-fc3b-4ecf-96dd-f7793d2ead4a", new DateTime(2021, 4, 28, 8, 17, 30, 923, DateTimeKind.Utc).AddTicks(6511), "AQAAAAEAACcQAAAAEPSo4PgopvsAhH0wYtGYImzHbc8djf4EvnTAK/7zBmJUZZp4BCALYCE7HOtEB0yJoA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsShowOnHome", "SortOrder" },
                values: new object[] { true, 1 });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsShowOnHome", "SortOrder" },
                values: new object[] { true, 2 });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 28, 8, 17, 30, 958, DateTimeKind.Utc).AddTicks(7125));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 28, 8, 17, 30, 957, DateTimeKind.Utc).AddTicks(7617));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 28, 8, 17, 30, 956, DateTimeKind.Utc).AddTicks(9733));
        }
    }
}
