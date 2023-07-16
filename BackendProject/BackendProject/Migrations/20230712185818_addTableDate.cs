using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendProject.Migrations
{
    public partial class addTableDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "SportOffs",
                newName: "Desc2");

            migrationBuilder.AddColumn<string>(
                name: "Desc1",
                table: "SportOffs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc1",
                table: "SportOffs");

            migrationBuilder.RenameColumn(
                name: "Desc2",
                table: "SportOffs",
                newName: "Desc");
        }
    }
}
