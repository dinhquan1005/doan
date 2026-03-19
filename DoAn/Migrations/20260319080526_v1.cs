using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Trips",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CustomerID",
                table: "Trips",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverID",
                table: "Trips",
                column: "DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_CustomerID",
                table: "Trips",
                column: "CustomerID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_DriverID",
                table: "Trips",
                column: "DriverID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_CustomerID",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_DriverID",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CustomerID",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_DriverID",
                table: "Trips");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
