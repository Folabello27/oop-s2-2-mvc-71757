using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71757.Data;
using oop_s2_2_mvc_71757.Models;

namespace oop_s2_2_mvc_71757.Controllers;

[Authorize(Roles = "Admin,Inspector,Viewer")]
public class FollowUpsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FollowUpsController> _logger;

    public FollowUpsController(ApplicationDbContext context, ILogger<FollowUpsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var followUps = await _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .AsNoTracking()
            .ToListAsync();
        return View(followUps);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var followUp = await _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (followUp is null)
        {
            return NotFound();
        }

        return View(followUp);
    }

    [Authorize(Roles = "Admin,Inspector")]
    public IActionResult Create()
    {
        ViewData["InspectionId"] = new SelectList(_context.Inspections.AsNoTracking(), "Id", "Id");
        return View();
    }

    [Authorize(Roles = "Admin,Inspector")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("InspectionId,DueDate,Status,ClosedDate")] FollowUp followUp)
    {
        var inspection = await _context.Inspections.AsNoTracking().FirstOrDefaultAsync(i => i.Id == followUp.InspectionId);
        if (inspection is not null && followUp.DueDate < inspection.InspectionDate)
        {
            ModelState.AddModelError(nameof(FollowUp.DueDate), "Due date cannot be before the inspection date.");
            _logger.LogWarning(
                "Follow-up due date {DueDate} before inspection date {InspectionDate} for Inspection {InspectionId}",
                followUp.DueDate,
                inspection.InspectionDate,
                followUp.InspectionId);
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Follow-up create failed validation for Inspection {InspectionId}", followUp.InspectionId);
            ViewData["InspectionId"] = new SelectList(_context.Inspections.AsNoTracking(), "Id", "Id", followUp.InspectionId);
            return View(followUp);
        }

        _context.Add(followUp);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Follow-up created {FollowUpId} for Inspection {InspectionId}", followUp.Id, followUp.InspectionId);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Inspector")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var followUp = await _context.FollowUps.FindAsync(id);
        if (followUp is null)
        {
            return NotFound();
        }

        ViewData["InspectionId"] = new SelectList(_context.Inspections.AsNoTracking(), "Id", "Id", followUp.InspectionId);
        return View(followUp);
    }

    [Authorize(Roles = "Admin,Inspector")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,InspectionId,DueDate,Status,ClosedDate")] FollowUp followUp)
    {
        if (id != followUp.Id)
        {
            return NotFound();
        }

        var inspection = await _context.Inspections.AsNoTracking().FirstOrDefaultAsync(i => i.Id == followUp.InspectionId);
        if (inspection is not null && followUp.DueDate < inspection.InspectionDate)
        {
            ModelState.AddModelError(nameof(FollowUp.DueDate), "Due date cannot be before the inspection date.");
            _logger.LogWarning(
                "Follow-up due date {DueDate} before inspection date {InspectionDate} for Inspection {InspectionId}",
                followUp.DueDate,
                inspection.InspectionDate,
                followUp.InspectionId);
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Follow-up edit failed validation for {FollowUpId}", followUp.Id);
            ViewData["InspectionId"] = new SelectList(_context.Inspections.AsNoTracking(), "Id", "Id", followUp.InspectionId);
            return View(followUp);
        }

        try
        {
            _context.Update(followUp);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Follow-up updated {FollowUpId} for Inspection {InspectionId}", followUp.Id, followUp.InspectionId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!FollowUpExists(followUp.Id))
            {
                return NotFound();
            }

            _logger.LogError(ex, "Follow-up update failed {FollowUpId}", followUp.Id);
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var followUp = await _context.FollowUps
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (followUp is null)
        {
            return NotFound();
        }

        return View(followUp);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var followUp = await _context.FollowUps.FindAsync(id);
        if (followUp is null)
        {
            return NotFound();
        }

        _context.FollowUps.Remove(followUp);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Follow-up deleted {FollowUpId} for Inspection {InspectionId}", followUp.Id, followUp.InspectionId);
        return RedirectToAction(nameof(Index));
    }

    private bool FollowUpExists(int id)
    {
        return _context.FollowUps.Any(e => e.Id == id);
    }
}
