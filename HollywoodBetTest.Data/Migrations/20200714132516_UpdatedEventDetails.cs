using Microsoft.EntityFrameworkCore.Migrations;

namespace HollywoodBetTest.Data.Migrations
{
    public partial class UpdatedEventDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "EventDetailNumber",
                table: "EventDetails",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDetailNumber",
                table: "EventDetails");
        }
    }
}
