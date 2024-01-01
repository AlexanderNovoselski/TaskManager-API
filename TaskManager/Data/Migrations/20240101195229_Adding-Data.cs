using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    public partial class AddingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("94e6d087-4651-4e6c-b157-ca83d366b193"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("ab4e6a35-29df-4d96-b9e4-b85a6ebb7ae4"));

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "72d72ae8-c79c-48e5-975c-5c70a150dbcf", "user1@example.com", true, true, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAEAACcQAAAAEBNItXFMQ3ndbhlWNbvGr1xei+jEvxIsFBlipzqt2xq5SJQDFwqI/I4vRvTdLnXADg==", null, false, "", false, "user1@example.com" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "OwnerId", "UpdatedDate" },
                values: new object[] { new Guid("13e848cd-b9d7-43ea-8042-cdb9f4fb8fb6"), new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6865), "This is a sample task.", new DateTime(2024, 1, 8, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6871), 1, false, "Sample Task 1", "1", new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6876) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "OwnerId", "UpdatedDate" },
                values: new object[] { new Guid("75b2ace2-37df-49fe-bbd9-19ebc3448846"), new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6877), "Another sample task.", new DateTime(2024, 1, 15, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6879), 0, false, "Sample Task 2", "1", new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6880) });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_OwnerId",
                table: "Tasks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_OwnerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks");

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("13e848cd-b9d7-43ea-8042-cdb9f4fb8fb6"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("75b2ace2-37df-49fe-bbd9-19ebc3448846"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tasks");

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "UpdatedDate" },
                values: new object[] { new Guid("94e6d087-4651-4e6c-b157-ca83d366b193"), new DateTime(2023, 12, 31, 12, 45, 24, 583, DateTimeKind.Utc).AddTicks(9732), "Another sample task.", new DateTime(2024, 1, 14, 12, 45, 24, 583, DateTimeKind.Utc).AddTicks(9734), 0, false, "Sample Task 2", new DateTime(2023, 12, 31, 12, 45, 24, 583, DateTimeKind.Utc).AddTicks(9735) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "UpdatedDate" },
                values: new object[] { new Guid("ab4e6a35-29df-4d96-b9e4-b85a6ebb7ae4"), new DateTime(2023, 12, 31, 12, 45, 24, 583, DateTimeKind.Utc).AddTicks(9693), "This is a sample task.", new DateTime(2024, 1, 7, 12, 45, 24, 583, DateTimeKind.Utc).AddTicks(9727), 1, false, "Sample Task 1", new DateTime(2023, 12, 31, 12, 45, 24, 583, DateTimeKind.Utc).AddTicks(9731) });
        }
    }
}
