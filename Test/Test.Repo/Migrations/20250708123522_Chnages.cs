using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.Repo.Migrations
{
    /// <inheritdoc />
    public partial class Chnages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "confirmPassword",
                table: "Users",
                newName: "ConfirmPassword");

            migrationBuilder.RenameColumn(
                name: "user_type",
                table: "Users",
                newName: "UserType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "ConfirmPassword",
                table: "Users",
                newName: "confirmPassword");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "user_type");
        }
    }
}
