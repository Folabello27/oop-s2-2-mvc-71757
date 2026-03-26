using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71757.Models;

namespace oop_s2_2_mvc_71757.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Premises> Premises => Set<Premises>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Premises>()
            .Property(p => p.RiskRating)
            .HasConversion<string>();

        builder.Entity<Inspection>()
            .Property(i => i.Outcome)
            .HasConversion<string>();

        builder.Entity<FollowUp>()
            .Property(f => f.Status)
            .HasConversion<string>();

        builder.Entity<Premises>()
            .HasMany(p => p.Inspections)
            .WithOne(i => i.Premises)
            .HasForeignKey(i => i.PremisesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Inspection>()
            .HasMany(i => i.FollowUps)
            .WithOne(f => f.Inspection)
            .HasForeignKey(f => f.InspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Premises>().HasData(
            new Premises { Id = 1, Name = "Harbor Grill", Address = "1 Ocean Rd", Town = "Portstown", RiskRating = RiskRating.Low },
            new Premises { Id = 2, Name = "Maple Cafe", Address = "12 Maple St", Town = "Portstown", RiskRating = RiskRating.Medium },
            new Premises { Id = 3, Name = "Night Market", Address = "88 Dock Ave", Town = "Portstown", RiskRating = RiskRating.High },
            new Premises { Id = 4, Name = "Pine Diner", Address = "7 Pine St", Town = "Lakeside", RiskRating = RiskRating.Medium },
            new Premises { Id = 5, Name = "Sunrise Bakery", Address = "21 Lake Rd", Town = "Lakeside", RiskRating = RiskRating.Low },
            new Premises { Id = 6, Name = "River Sushi", Address = "9 River Walk", Town = "Lakeside", RiskRating = RiskRating.High },
            new Premises { Id = 7, Name = "Hilltop BBQ", Address = "3 Summit Dr", Town = "Hillview", RiskRating = RiskRating.Medium },
            new Premises { Id = 8, Name = "Garden Bistro", Address = "14 Garden Ln", Town = "Hillview", RiskRating = RiskRating.Low },
            new Premises { Id = 9, Name = "Central Pub", Address = "101 Main St", Town = "Hillview", RiskRating = RiskRating.High },
            new Premises { Id = 10, Name = "Green Leaf", Address = "5 Orchard Rd", Town = "Portstown", RiskRating = RiskRating.Low },
            new Premises { Id = 11, Name = "Spice Route", Address = "66 Market St", Town = "Lakeside", RiskRating = RiskRating.High },
            new Premises { Id = 12, Name = "Coastal Eats", Address = "2 Bay Blvd", Town = "Hillview", RiskRating = RiskRating.Medium }
        );

        builder.Entity<Inspection>().HasData(
            new Inspection { Id = 1, PremisesId = 1, InspectionDate = new DateTime(2026, 1, 10), Score = 92, Outcome = InspectionOutcome.Pass, Notes = "Routine inspection." },
            new Inspection { Id = 2, PremisesId = 2, InspectionDate = new DateTime(2026, 1, 15), Score = 78, Outcome = InspectionOutcome.Pass, Notes = "Minor issues corrected." },
            new Inspection { Id = 3, PremisesId = 3, InspectionDate = new DateTime(2026, 1, 20), Score = 55, Outcome = InspectionOutcome.Fail, Notes = "Cooling logs missing." },
            new Inspection { Id = 4, PremisesId = 4, InspectionDate = new DateTime(2026, 1, 28), Score = 88, Outcome = InspectionOutcome.Pass, Notes = "Good hygiene practices." },
            new Inspection { Id = 5, PremisesId = 5, InspectionDate = new DateTime(2026, 2, 3), Score = 67, Outcome = InspectionOutcome.Pass, Notes = "Minor storage adjustments." },
            new Inspection { Id = 6, PremisesId = 6, InspectionDate = new DateTime(2026, 2, 5), Score = 49, Outcome = InspectionOutcome.Fail, Notes = "Cross-contamination risk." },
            new Inspection { Id = 7, PremisesId = 7, InspectionDate = new DateTime(2026, 2, 7), Score = 73, Outcome = InspectionOutcome.Pass, Notes = "Records up to date." },
            new Inspection { Id = 8, PremisesId = 8, InspectionDate = new DateTime(2026, 2, 10), Score = 95, Outcome = InspectionOutcome.Pass, Notes = "Excellent standards." },
            new Inspection { Id = 9, PremisesId = 9, InspectionDate = new DateTime(2026, 2, 12), Score = 41, Outcome = InspectionOutcome.Fail, Notes = "Cleaning schedule lapsed." },
            new Inspection { Id = 10, PremisesId = 10, InspectionDate = new DateTime(2026, 2, 14), Score = 82, Outcome = InspectionOutcome.Pass, Notes = "Routine checks completed." },
            new Inspection { Id = 11, PremisesId = 11, InspectionDate = new DateTime(2026, 2, 18), Score = 52, Outcome = InspectionOutcome.Fail, Notes = "Temperature control issues." },
            new Inspection { Id = 12, PremisesId = 12, InspectionDate = new DateTime(2026, 2, 20), Score = 86, Outcome = InspectionOutcome.Pass, Notes = "Good practice overall." },
            new Inspection { Id = 13, PremisesId = 1, InspectionDate = new DateTime(2026, 3, 2), Score = 90, Outcome = InspectionOutcome.Pass, Notes = "Follow-up spot check." },
            new Inspection { Id = 14, PremisesId = 2, InspectionDate = new DateTime(2026, 3, 3), Score = 63, Outcome = InspectionOutcome.Pass, Notes = "Minor labeling fixes." },
            new Inspection { Id = 15, PremisesId = 3, InspectionDate = new DateTime(2026, 3, 4), Score = 58, Outcome = InspectionOutcome.Fail, Notes = "Handwashing signage missing." },
            new Inspection { Id = 16, PremisesId = 4, InspectionDate = new DateTime(2026, 3, 6), Score = 80, Outcome = InspectionOutcome.Pass, Notes = "No major concerns." },
            new Inspection { Id = 17, PremisesId = 5, InspectionDate = new DateTime(2026, 3, 8), Score = 76, Outcome = InspectionOutcome.Pass, Notes = "Storage corrected." },
            new Inspection { Id = 18, PremisesId = 6, InspectionDate = new DateTime(2026, 3, 10), Score = 44, Outcome = InspectionOutcome.Fail, Notes = "Staff training overdue." },
            new Inspection { Id = 19, PremisesId = 7, InspectionDate = new DateTime(2026, 3, 12), Score = 70, Outcome = InspectionOutcome.Pass, Notes = "Satisfactory." },
            new Inspection { Id = 20, PremisesId = 8, InspectionDate = new DateTime(2026, 3, 15), Score = 96, Outcome = InspectionOutcome.Pass, Notes = "Excellent." },
            new Inspection { Id = 21, PremisesId = 9, InspectionDate = new DateTime(2026, 3, 17), Score = 39, Outcome = InspectionOutcome.Fail, Notes = "Pest control documentation missing." },
            new Inspection { Id = 22, PremisesId = 10, InspectionDate = new DateTime(2026, 3, 18), Score = 85, Outcome = InspectionOutcome.Pass, Notes = "Good compliance." },
            new Inspection { Id = 23, PremisesId = 11, InspectionDate = new DateTime(2026, 3, 19), Score = 61, Outcome = InspectionOutcome.Pass, Notes = "Improved controls." },
            new Inspection { Id = 24, PremisesId = 12, InspectionDate = new DateTime(2026, 3, 20), Score = 48, Outcome = InspectionOutcome.Fail, Notes = "Equipment maintenance needed." },
            new Inspection { Id = 25, PremisesId = 1, InspectionDate = new DateTime(2026, 3, 22), Score = 77, Outcome = InspectionOutcome.Pass, Notes = "Routine spot check." }
        );

        builder.Entity<FollowUp>().HasData(
            new FollowUp { Id = 1, InspectionId = 3, DueDate = new DateTime(2026, 2, 5), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 2, 4) },
            new FollowUp { Id = 2, InspectionId = 6, DueDate = new DateTime(2026, 2, 20), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 3, InspectionId = 9, DueDate = new DateTime(2026, 3, 1), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 3, 2) },
            new FollowUp { Id = 4, InspectionId = 11, DueDate = new DateTime(2026, 3, 5), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 5, InspectionId = 15, DueDate = new DateTime(2026, 3, 12), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 6, InspectionId = 18, DueDate = new DateTime(2026, 3, 20), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 7, InspectionId = 21, DueDate = new DateTime(2026, 4, 1), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 8, InspectionId = 24, DueDate = new DateTime(2026, 4, 5), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 9, InspectionId = 3, DueDate = new DateTime(2026, 1, 30), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 1, 25) },
            new FollowUp { Id = 10, InspectionId = 6, DueDate = new DateTime(2026, 3, 25), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 3, 24) }
        );
    }
}
