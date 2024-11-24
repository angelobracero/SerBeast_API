using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerBeast_API.Migrations
{
    /// <inheritdoc />
    public partial class addColumnsToProfessionalService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ProfessionalServices_ProfessionalServiceId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ProfessionalServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProfessionalServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProfessionalServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalServices_CategoryId",
                table: "ProfessionalServices",
                column: "CategoryId");

            // Correct Foreign Key reference
            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ProfessionalServices_ProfessionalServiceId",
                table: "Bookings",
                column: "ProfessionalServiceId",
                principalTable: "ProfessionalServices",
                principalColumn: "ProfessionalServiceId");

            // Corrected Foreign Key reference for Categories
            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalServices_Categories_CategoryId",
                table: "ProfessionalServices",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ProfessionalServices_ProfessionalServiceId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalServices_Categories_CategoryId",
                table: "ProfessionalServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_ProfessionalServices_CategoryId",
                table: "ProfessionalServices");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProfessionalServices");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProfessionalServices");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProfessionalServices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "AspNetUsers",
                type: "decimal(4,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ProfessionalServices_ProfessionalServiceId",
                table: "Bookings",
                column: "ProfessionalServiceId",
                principalTable: "ProfessionalServices",
                principalColumn: "ProfessionalServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
