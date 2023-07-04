using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalR_Complete.Data.Migrations
{
    public partial class mg4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReceiver",
                table: "PrivateMessage",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSender",
                table: "PrivateMessage",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReceiver",
                table: "PrivateMessage");

            migrationBuilder.DropColumn(
                name: "IsSender",
                table: "PrivateMessage");
        }
    }
}
