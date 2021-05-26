using Microsoft.EntityFrameworkCore.Migrations;

namespace DepartmentManagment.Migrations
{
    public partial class AddImageForDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Departments");
        }
    }
}
