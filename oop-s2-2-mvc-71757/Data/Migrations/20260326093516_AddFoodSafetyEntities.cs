using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace oop_s2_2_mvc_71757.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodSafetyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Premises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Town = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    RiskRating = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PremisesId = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Outcome = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Premises_PremisesId",
                        column: x => x.PremisesId,
                        principalTable: "Premises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InspectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUps_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Premises",
                columns: new[] { "Id", "Address", "Name", "RiskRating", "Town" },
                values: new object[,]
                {
                    { 1, "1 Ocean Rd", "Harbor Grill", "Low", "Portstown" },
                    { 2, "12 Maple St", "Maple Cafe", "Medium", "Portstown" },
                    { 3, "88 Dock Ave", "Night Market", "High", "Portstown" },
                    { 4, "7 Pine St", "Pine Diner", "Medium", "Lakeside" },
                    { 5, "21 Lake Rd", "Sunrise Bakery", "Low", "Lakeside" },
                    { 6, "9 River Walk", "River Sushi", "High", "Lakeside" },
                    { 7, "3 Summit Dr", "Hilltop BBQ", "Medium", "Hillview" },
                    { 8, "14 Garden Ln", "Garden Bistro", "Low", "Hillview" },
                    { 9, "101 Main St", "Central Pub", "High", "Hillview" },
                    { 10, "5 Orchard Rd", "Green Leaf", "Low", "Portstown" },
                    { 11, "66 Market St", "Spice Route", "High", "Lakeside" },
                    { 12, "2 Bay Blvd", "Coastal Eats", "Medium", "Hillview" }
                });

            migrationBuilder.InsertData(
                table: "Inspections",
                columns: new[] { "Id", "InspectionDate", "Notes", "Outcome", "PremisesId", "Score" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Routine inspection.", "Pass", 1, 92 },
                    { 2, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minor issues corrected.", "Pass", 2, 78 },
                    { 3, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cooling logs missing.", "Fail", 3, 55 },
                    { 4, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good hygiene practices.", "Pass", 4, 88 },
                    { 5, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minor storage adjustments.", "Pass", 5, 67 },
                    { 6, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cross-contamination risk.", "Fail", 6, 49 },
                    { 7, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Records up to date.", "Pass", 7, 73 },
                    { 8, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Excellent standards.", "Pass", 8, 95 },
                    { 9, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cleaning schedule lapsed.", "Fail", 9, 41 },
                    { 10, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Routine checks completed.", "Pass", 10, 82 },
                    { 11, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Temperature control issues.", "Fail", 11, 52 },
                    { 12, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good practice overall.", "Pass", 12, 86 },
                    { 13, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Follow-up spot check.", "Pass", 1, 90 },
                    { 14, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minor labeling fixes.", "Pass", 2, 63 },
                    { 15, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Handwashing signage missing.", "Fail", 3, 58 },
                    { 16, new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "No major concerns.", "Pass", 4, 80 },
                    { 17, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Storage corrected.", "Pass", 5, 76 },
                    { 18, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Staff training overdue.", "Fail", 6, 44 },
                    { 19, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Satisfactory.", "Pass", 7, 70 },
                    { 20, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Excellent.", "Pass", 8, 96 },
                    { 21, new DateTime(2026, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pest control documentation missing.", "Fail", 9, 39 },
                    { 22, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good compliance.", "Pass", 10, 85 },
                    { 23, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Improved controls.", "Pass", 11, 61 },
                    { 24, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Equipment maintenance needed.", "Fail", 12, 48 },
                    { 25, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Routine spot check.", "Pass", 1, 77 }
                });

            migrationBuilder.InsertData(
                table: "FollowUps",
                columns: new[] { "Id", "ClosedDate", "DueDate", "InspectionId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Closed" },
                    { 2, null, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Open" },
                    { 3, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Closed" },
                    { 4, null, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Open" },
                    { 5, null, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "Open" },
                    { 6, null, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, "Open" },
                    { 7, null, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, "Open" },
                    { 8, null, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, "Open" },
                    { 9, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Closed" },
                    { 10, new DateTime(2026, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Closed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_InspectionId",
                table: "FollowUps",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PremisesId",
                table: "Inspections",
                column: "PremisesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUps");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "Premises");
        }
    }
}
