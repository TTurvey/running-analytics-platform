using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RunningAnalytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("8b7d3a1c-0e5f-4c63-9e91-1a2b3c4d5e6f"), new DateTime(2026, 8, 1, 8, 0, 0, 0, DateTimeKind.Utc), "testuser1@example.com", "TestUser 1", "TEST_ONLY_HASH_1" },
                    { new Guid("9c8e4b2d-1f60-5d74-af02-2b3c4d5e6f70"), new DateTime(2026, 8, 2, 9, 30, 0, 0, DateTimeKind.Utc), "testuser2@example.com", "TestUser 2", "TEST_ONLY_HASH_2" },
                    { new Guid("ad9f5c3e-2071-6e85-b013-3c4d5e6f7081"), new DateTime(2026, 8, 3, 10, 15, 0, 0, DateTimeKind.Utc), "testuser3@example.com", "TestUser 3", "TEST_ONLY_HASH_3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8b7d3a1c-0e5f-4c63-9e91-1a2b3c4d5e6f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9c8e4b2d-1f60-5d74-af02-2b3c4d5e6f70"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ad9f5c3e-2071-6e85-b013-3c4d5e6f7081"));
        }
    }
}
