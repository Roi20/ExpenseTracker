using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDeleteOnRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers",
                table: "Transactions",
                column: "User_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Categories",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Categories",
                table: "Transactions");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers",
                table: "Transactions",
                column: "User_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }
    }
}
