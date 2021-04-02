using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Censors_Persons_PersonId",
                table: "Censors");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AppUsers_UserId",
                table: "Persons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "UserInfos");

            migrationBuilder.RenameIndex(
                name: "IX_Persons_UserId",
                table: "UserInfos",
                newName: "IX_UserInfos_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "75dd60e3-e152-4d05-8d01-c5f78ddfeed0");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Censors_UserInfos_PersonId",
                table: "Censors",
                column: "PersonId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_AppUsers_UserId",
                table: "UserInfos",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Censors_UserInfos_PersonId",
                table: "Censors");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_AppUsers_UserId",
                table: "UserInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos");

            migrationBuilder.RenameTable(
                name: "UserInfos",
                newName: "Persons");

            migrationBuilder.RenameIndex(
                name: "IX_UserInfos_UserId",
                table: "Persons",
                newName: "IX_Persons_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "2805acca-c5b1-47da-aaf8-4a1c51470d13");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "76aaa138-a1f6-40dd-9249-960000854c14", "AQAAAAEAACcQAAAAEA8UgSPQEdhd34S5OCek88e7ajc/yL9IOJadoyWx0pADtHsaPtNAVWQt6hqDyJ72FQ==" });

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 2, 11, 37, 33, 485, DateTimeKind.Local).AddTicks(9239));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 2, 11, 37, 33, 484, DateTimeKind.Local).AddTicks(1625));

            migrationBuilder.AddForeignKey(
                name: "FK_Censors_Persons_PersonId",
                table: "Censors",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AppUsers_UserId",
                table: "Persons",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
