using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.Repo.Migrations
{
    /// <inheritdoc />
    public partial class modifiableadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails");

            migrationBuilder.RenameTable(
                name: "UserDetails",
                newName: "UserDetailsList");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetailsList",
                table: "UserDetailsList",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetailsList",
                table: "UserDetailsList");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserDetailsList",
                newName: "UserDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails",
                column: "Id");
        }
    }
}
