using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Impactt.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "booked_times",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<long>(type: "bigint", nullable: false),
                    resident = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_booked_times", x => x.id);
                    table.ForeignKey(
                        name: "fk_booked_times_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "rooms",
                columns: new[] { "id", "capacity", "name", "type" },
                values: new object[,]
                {
                    { 1L, 1, "mytaxi", "focus" },
                    { 2L, 5, "workly", "team" },
                    { 3L, 15, "express24", "conference" },
                    { 4L, 4, "amazon", "focus" },
                    { 5L, 10, "google", "team" },
                    { 6L, 24, "meta", "conference" },
                    { 7L, 2, "uber", "focus" },
                    { 8L, 20, "twitter", "conference" },
                    { 9L, 3, "apple", "focus" },
                    { 10L, 6, "microsoft", "team" },
                    { 11L, 18, "yandex", "conference" },
                    { 12L, 2, "yahoo", "focus" },
                    { 13L, 7, "oracle", "team" },
                    { 14L, 16, "intel", "conference" },
                    { 15L, 2, "ibm", "focus" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_booked_times_end_time",
                table: "booked_times",
                column: "end_time");

            migrationBuilder.CreateIndex(
                name: "ix_booked_times_room_id",
                table: "booked_times",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_booked_times_start_time",
                table: "booked_times",
                column: "start_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booked_times");

            migrationBuilder.DropTable(
                name: "rooms");
        }
    }
}
