using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71757.Data;
using oop_s2_2_mvc_71757.Models;

namespace oop_s2_2_mvc_71757.Controllers;

[Authorize(Roles = "Admin,Inspector,Viewer")]
public class InspectionsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<InspectionsController> _logger;

    public InspectionsController(ApplicationDbContext context, ILogger<InspectionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var inspections = await _context.Inspections
            .Include(i => i.Premises)
            .AsNoTracking()
            .ToListAsync();
        return View(inspections);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var inspection = await _context.Inspections
            .Include(i => i.Premises)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (inspection is null)
        {
            return NotFound();
        }

        return View(inspection);
    }

    [Authorize(Roles = "Admin,Inspector")]
    public IActionResult Create()
    {
        ViewData["PremisesId"] = new SelectList(_context.Premises.AsNoTracking(), "Id", "Name");
        return View();
    }

    [Authorize(Roles = "Admin,Inspector")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PremisesId,InspectionDate,Score,Outcome,Notes")] Inspection inspection)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Inspection create failed validation for Premises {PremisesId}", inspection.PremisesId);
            ViewData["PremisesId"] = new SelectList(_context.Premises.AsNoTracking(), "Id", "Name", inspection.PremisesId);
            return View(inspection);
        }

        _context.Add(inspection);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Inspection created {InspectionId} for Premises {PremisesId}", inspection.Id, inspection.PremisesId);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Inspector")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var inspection = await _context.Inspections.FindAsync(id);
        if (inspection is null)
        {
            return NotFound();
        }

        ViewData["PremisesId"] = new SelectList(_context.Premises.AsNoTracking(), "Id", "Name", inspection.PremisesId);
        return View(inspection);
    }

    [Authorize(Roles = "Admin,Inspector")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,PremisesId,InspectionDate,Score,Outcome,Notes")] Inspection inspection)
    {
        if (id != inspection.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Inspection edit failed validation for {InspectionId}", inspection.Id);
            ViewData["PremisesId"] = new SelectList(_context.Premises.AsNoTracking(), "Id", "Name", inspection.PremisesId);
            return View(inspection);
        }

        try
        {
            _context.Update(inspection);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Inspection updated {InspectionId} for Premises {PremisesId}", inspection.Id, inspection.PremisesId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!InspectionExists(inspection.Id))
            {
                return NotFound();
            }

            _logger.LogError(ex, "Inspection update failed {InspectionId}", inspection.Id);
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

        var inspection = await _context.Inspections
            .Include(i => i.Premises)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (inspection is null)
        {
            return NotFound();
        }

        return View(inspection);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inspection = await _context.Inspections.FindAsync(id);
        if (inspection is null)
        {
            return NotFound();
        }

        _context.Inspections.Remove(inspection);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Inspection deleted {InspectionId} for Premises {PremisesId}", inspection.Id, inspection.PremisesId);
        return RedirectToAction(nameof(Index));
    }

    private bool InspectionExists(int id)
    {
        return _context.Inspections.Any(e => e.Id == id);
    }
}
