using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71757.Data;
using oop_s2_2_mvc_71757.Models;
using oop_s2_2_mvc_71757.ViewModels;

namespace oop_s2_2_mvc_71757.Controllers;

[Authorize(Roles = "Admin,Inspector,Viewer")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? town, RiskRating? riskRating)
    {
        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        var inspectionQuery = _context.Inspections
            .Include(i => i.Premises)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(town))
        {
            inspectionQuery = inspectionQuery.Where(i => i.Premises!.Town == town);
        }

        if (riskRating is not null)
        {
            inspectionQuery = inspectionQuery.Where(i => i.Premises!.RiskRating == riskRating);
        }

        var inspectionsThisMonth = await inspectionQuery
            .Where(i => i.InspectionDate >= monthStart && i.InspectionDate < monthEnd)
            .CountAsync();

        var failedInspectionsThisMonth = await inspectionQuery
            .Where(i => i.InspectionDate >= monthStart && i.InspectionDate < monthEnd)
            .Where(i => i.Outcome == InspectionOutcome.Fail)
            .CountAsync();

        var followUpQuery = _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(town))
        {
            followUpQuery = followUpQuery.Where(f => f.Inspection!.Premises!.Town == town);
        }

        if (riskRating is not null)
        {
            followUpQuery = followUpQuery.Where(f => f.Inspection!.Premises!.RiskRating == riskRating);
        }

        var overdueOpenFollowUps = await followUpQuery
            .Where(f => f.Status == FollowUpStatus.Open && f.DueDate < today)
            .CountAsync();

        var towns = await _context.Premises
            .AsNoTracking()
            .Select(p => p.Town)
            .Distinct()
            .OrderBy(t => t)
            .ToListAsync();

        var model = new DashboardViewModel
        {
            InspectionsThisMonth = inspectionsThisMonth,
            FailedInspectionsThisMonth = failedInspectionsThisMonth,
            OverdueOpenFollowUps = overdueOpenFollowUps,
            SelectedTown = town,
            SelectedRiskRating = riskRating,
            Towns = towns
        };

        _logger.LogInformation(
            "Dashboard viewed with filters Town={Town} RiskRating={RiskRating}",
            town ?? "All",
            riskRating?.ToString() ?? "All");

        return View(model);
    }
}
