﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agrigate.Authentication.Migrations
{
    /// <inheritdoc />
    public partial class AddUsername_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Username",
                table: "User");
        }
    }
}
