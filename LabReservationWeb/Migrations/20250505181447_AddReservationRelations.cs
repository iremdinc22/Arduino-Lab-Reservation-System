﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabReservation.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Labs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Delek1");

            migrationBuilder.UpdateData(
                table: "Labs",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Delek2");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_UserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "Labs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "delek1");

            migrationBuilder.UpdateData(
                table: "Labs",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "delek2");
        }
    }
}
