using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminNotifIdCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AdminNotifications",
                newName: "AdminNotificationId");

            migrationBuilder.AddColumn<int>(
                name: "AdminNotificationId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNotificationId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "AdminNotificationId",
                table: "AdminNotifications",
                newName: "Id");
        }
    }
}
