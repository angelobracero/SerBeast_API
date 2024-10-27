using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SerBeast_API.Migrations
{
    /// <inheritdoc />
    public partial class seedServiceToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CategoryId", "Description", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Repairs, installations, and emergency services.", "Plumbing" },
                    { 2, 1, "Wiring, installations, and troubleshooting.", "Electrical Work" },
                    { 3, 1, "Residential, commercial, deep cleaning, and move-in/move-out cleaning.", "Cleaning Services" },
                    { 4, 1, "Gardening, lawn care, and landscaping design.", "Landscaping" },
                    { 5, 1, "Repairs, installations, and inspections.", "Roofing" },
                    { 6, 1, "Heating, ventilation, and air conditioning maintenance and installation.", "HVAC Services" },
                    { 7, 1, "Extermination and prevention services.", "Pest Control" },
                    { 8, 1, "Interior and exterior painting services.", "Painting" },
                    { 9, 1, "Custom furniture, repairs, and installations.", "Carpentry" },
                    { 10, 2, "Academic tutoring and test preparation.", "Tutoring" },
                    { 11, 2, "Business consulting for startups or management.", "Consulting" },
                    { 12, 2, "Document preparation, legal advice, and representation.", "Legal Services" },
                    { 13, 2, "Accounting, bookkeeping, and financial planning.", "Financial Services" },
                    { 14, 3, "Haircuts, styling, makeup, and skincare.", "Beauty Services" },
                    { 15, 3, "Personal training, yoga, and fitness classes.", "Fitness Training" },
                    { 16, 3, "Babysitting, nanny services, and daycare.", "Childcare" },
                    { 17, 4, "Event catering for parties, weddings, and corporate events.", "Catering" },
                    { 18, 4, "Professional photography services for events and portraits.", "Photography" },
                    { 19, 4, "Planning and coordinating events.", "Event Planning" },
                    { 20, 5, "Computer repairs, tech support, and network setup.", "IT Support" },
                    { 21, 5, "Website design, development, and maintenance.", "Web Development" },
                    { 22, 5, "Branding, logo design, and marketing materials.", "Graphic Design" },
                    { 23, 6, "General maintenance and repairs.", "Car Repairs" },
                    { 24, 6, "Car washing and detailing.", "Detailing Services" },
                    { 25, 7, "Dog walking, pet sitting, and grooming.", "Pet Services" },
                    { 26, 7, "Packing, loading, and transporting belongings.", "Moving Services" },
                    { 27, 7, "Home staging and interior design consultations.", "Interior Design" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 27);
        }
    }
}
