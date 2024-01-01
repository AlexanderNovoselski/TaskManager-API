using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    public partial class SettingRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("13e848cd-b9d7-43ea-8042-cdb9f4fb8fb6"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("75b2ace2-37df-49fe-bbd9-19ebc3448846"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "805361ef-1fe8-47f7-8f4f-78c7dfeb86a2", "AQAAAAEAACcQAAAAEFtWXa6P+ajPH13H08CsS8CJDpb5htNj0L1fyG64hupmP7O8IvtS1QpgXecrJwV3aw==" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "OwnerId", "UpdatedDate" },
                values: new object[] { new Guid("2e98d6dc-9457-443b-bf5d-d3ce34ea198f"), new DateTime(2024, 1, 1, 20, 1, 9, 477, DateTimeKind.Utc).AddTicks(4398), "Another sample task.", new DateTime(2024, 1, 15, 20, 1, 9, 477, DateTimeKind.Utc).AddTicks(4400), 0, false, "Sample Task 2", "1", new DateTime(2024, 1, 1, 20, 1, 9, 477, DateTimeKind.Utc).AddTicks(4400) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "OwnerId", "UpdatedDate" },
                values: new object[] { new Guid("fb50d0b8-c79f-4606-89e0-285e0d71a40e"), new DateTime(2024, 1, 1, 20, 1, 9, 477, DateTimeKind.Utc).AddTicks(4381), "This is a sample task.", new DateTime(2024, 1, 8, 20, 1, 9, 477, DateTimeKind.Utc).AddTicks(4388), 1, false, "Sample Task 1", "1", new DateTime(2024, 1, 1, 20, 1, 9, 477, DateTimeKind.Utc).AddTicks(4397) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("2e98d6dc-9457-443b-bf5d-d3ce34ea198f"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("fb50d0b8-c79f-4606-89e0-285e0d71a40e"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "72d72ae8-c79c-48e5-975c-5c70a150dbcf", "AQAAAAEAACcQAAAAEBNItXFMQ3ndbhlWNbvGr1xei+jEvxIsFBlipzqt2xq5SJQDFwqI/I4vRvTdLnXADg==" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "OwnerId", "UpdatedDate" },
                values: new object[] { new Guid("13e848cd-b9d7-43ea-8042-cdb9f4fb8fb6"), new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6865), "This is a sample task.", new DateTime(2024, 1, 8, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6871), 1, false, "Sample Task 1", "1", new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6876) });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AddedDate", "Description", "DueDate", "ImportanceLevel", "IsCompleted", "Name", "OwnerId", "UpdatedDate" },
                values: new object[] { new Guid("75b2ace2-37df-49fe-bbd9-19ebc3448846"), new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6877), "Another sample task.", new DateTime(2024, 1, 15, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6879), 0, false, "Sample Task 2", "1", new DateTime(2024, 1, 1, 19, 52, 29, 533, DateTimeKind.Utc).AddTicks(6880) });
        }
    }
}
