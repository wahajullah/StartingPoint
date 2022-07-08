using Microsoft.EntityFrameworkCore.Migrations;

namespace StartingPoint.Migrations
{
    public partial class addEnqiry1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Enquiry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "Enquiry",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Enquiry");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "Enquiry");
        }
    }
}
