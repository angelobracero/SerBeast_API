using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerBeast_API.Migrations
{
    /// <inheritdoc />
    public partial class something : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ProfessionalServices_ProfessionalServiceId",
                table: "Bookings",
                column: "ProfessionalServiceId",
                principalTable: "ProfessionalServices",
                principalColumn: "ProfessionalServiceId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ProfessionalServices_ProfessionalServiceId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");
        }
    }
}
