using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddFKOnNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AdminNotificationId",
                table: "Notifications",
                column: "AdminNotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AdminNotifications_AdminNotificationId",
                table: "Notifications",
                column: "AdminNotificationId",
                principalTable: "AdminNotifications",
                principalColumn: "AdminNotificationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AdminNotifications_AdminNotificationId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AdminNotificationId",
                table: "Notifications");
        }
    }
}
