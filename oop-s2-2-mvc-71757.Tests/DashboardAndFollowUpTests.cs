using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using oop_s2_2_mvc_71757.Controllers;
using oop_s2_2_mvc_71757.Data;
using oop_s2_2_mvc_71757.Models;
using oop_s2_2_mvc_71757.ViewModels;

namespace oop_s2_2_mvc_71757.Tests;

public class DashboardAndFollowUpTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void FollowUp_requires_closed_date_when_closed()
    {
        var followUp = new FollowUp
        {
            InspectionId = 1,
            DueDate = DateTime.Today.AddDays(7),
            Status = FollowUpStatus.Closed,
            ClosedDate = null
        };

        var context = new ValidationContext(followUp);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(followUp, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(FollowUp.ClosedDate)));
    }

    [Fact]
    public async Task Overdue_followups_query_returns_only_overdue_open()
    {
        await using var context = CreateContext();
        var premises = new Premises { Name = "Test", Address = "1 Road", Town = "Town", RiskRating = RiskRating.Low };
        context.Premises.Add(premises);
        await context.SaveChangesAsync();

        var inspection = new Inspection
        {
            PremisesId = premises.Id,
            InspectionDate = DateTime.Today.AddDays(-10),
            Score = 50,
            Outcome = InspectionOutcome.Fail
        };
        context.Inspections.Add(inspection);
        await context.SaveChangesAsync();

        context.FollowUps.AddRange(
            new FollowUp { InspectionId = inspection.Id, DueDate = DateTime.Today.AddDays(-1), Status = FollowUpStatus.Open },
            new FollowUp { InspectionId = inspection.Id, DueDate = DateTime.Today.AddDays(3), Status = FollowUpStatus.Open },
            new FollowUp { InspectionId = inspection.Id, DueDate = DateTime.Today.AddDays(-2), Status = FollowUpStatus.Closed, ClosedDate = DateTime.Today.AddDays(-1) }
        );
        await context.SaveChangesAsync();

        var overdue = await context.FollowUps
            .Where(f => f.Status == FollowUpStatus.Open && f.DueDate < DateTime.Today)
            .ToListAsync();

        Assert.Single(overdue);
        Assert.Equal(FollowUpStatus.Open, overdue[0].Status);
    }

    [Fact]
    public async Task Dashboard_counts_match_expected_values()
    {
        await using var context = CreateContext();
        var premises = new Premises { Name = "Test", Address = "1 Road", Town = "Town", RiskRating = RiskRating.Medium };
        context.Premises.Add(premises);
        await context.SaveChangesAsync();

        var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var inspection1 = new Inspection { PremisesId = premises.Id, InspectionDate = monthStart.AddDays(1), Score = 90, Outcome = InspectionOutcome.Pass };
        var inspection2 = new Inspection { PremisesId = premises.Id, InspectionDate = monthStart.AddDays(2), Score = 40, Outcome = InspectionOutcome.Fail };
        var inspection3 = new Inspection { PremisesId = premises.Id, InspectionDate = monthStart.AddMonths(-1).AddDays(1), Score = 60, Outcome = InspectionOutcome.Pass };

        context.Inspections.AddRange(inspection1, inspection2, inspection3);
        await context.SaveChangesAsync();

        context.FollowUps.AddRange(
            new FollowUp { InspectionId = inspection1.Id, DueDate = DateTime.Today.AddDays(-1), Status = FollowUpStatus.Open },
            new FollowUp { InspectionId = inspection2.Id, DueDate = DateTime.Today.AddDays(2), Status = FollowUpStatus.Open }
        );
        await context.SaveChangesAsync();

        var controller = new DashboardController(context, NullLogger<DashboardController>.Instance);
        var result = await controller.Index(null, null) as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsType<DashboardViewModel>(result!.Model);
        Assert.Equal(2, model.InspectionsThisMonth);
        Assert.Equal(1, model.FailedInspectionsThisMonth);
        Assert.Equal(1, model.OverdueOpenFollowUps);
    }

    [Fact]
    public void Premises_controller_has_role_based_authorization()
    {
        var controllerAttributes = typeof(PremisesController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .ToList();

        Assert.Contains(controllerAttributes, attr => attr.Roles == "Admin,Inspector,Viewer");

        var createMethod = typeof(PremisesController).GetMethod(nameof(PremisesController.Create), new Type[] { });
        var createAttributes = createMethod!
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .ToList();

        Assert.Contains(createAttributes, attr => attr.Roles == "Admin");
    }
}
