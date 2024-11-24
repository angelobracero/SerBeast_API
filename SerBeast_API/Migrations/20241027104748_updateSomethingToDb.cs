using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerBeast_API.Migrations
{
    /// <inheritdoc />
    public partial class updateSomethingToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
        name: "NewBookings",
        columns: table => new
        {
            Id = table.Column<string>(type: "nvarchar(450)", nullable: false), // Adjust size as necessary
            UserId = table.Column<string>(type: "nvarchar(450)", nullable: true), // Update if needed
            ProfessionalServiceId = table.Column<int>(type: "int", nullable: false),
            BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
            CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: DateTime.Now) // Default to current date
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_NewBookings", x => x.Id);
            // Add foreign key constraints if necessary
        });

            // Step 2: Copy data from the old table to the new table
            migrationBuilder.Sql("INSERT INTO NewBookings (Id, UserId, ProfessionalServiceId, BookingDate) SELECT CAST(Id AS NVARCHAR(450)), UserId, ProfessionalServiceId, BookingDate FROM Bookings");

            // Step 3: Drop the old table
            migrationBuilder.DropTable(name: "Bookings");

            // Step 4: Rename the new table to the original table name
            migrationBuilder.RenameTable(name: "NewBookings", newName: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
        name: "OldBookings",
        columns: table => new
        {
            Id = table.Column<int>(type: "int")
                .Annotation("SqlServer:Identity", "1, 1"), // Restore the old identity column
            UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            ProfessionalServiceId = table.Column<int>(type: "int", nullable: false),
            BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
            CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_OldBookings", x => x.Id);
        });

            // Step 2: Copy data back from the new table to the old table
            migrationBuilder.Sql("INSERT INTO OldBookings (Id, UserId, ProfessionalServiceId, BookingDate) SELECT CAST(Id AS INT), UserId, ProfessionalServiceId, BookingDate FROM Bookings");

            // Step 3: Drop the new table
            migrationBuilder.DropTable(name: "Bookings");

            // Step 4: Rename the old table back to the original
            migrationBuilder.RenameTable(name: "OldBookings", newName: "Bookings");
        }
    }
}
