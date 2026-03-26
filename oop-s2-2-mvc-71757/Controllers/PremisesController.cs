using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71757.Data;
using oop_s2_2_mvc_71757.Models;

namespace oop_s2_2_mvc_71757.Controllers;

[Authorize(Roles = "Admin,Inspector,Viewer")]
public class PremisesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PremisesController> _logger;

    public PremisesController(ApplicationDbContext context, ILogger<PremisesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var premises = await _context.Premises.AsNoTracking().ToListAsync();
        return View(premises);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var premises = await _context.Premises.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (premises is null)
        {
            return NotFound();
        }

        return View(premises);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Address,Town,RiskRating")] Premises premises)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Premises create failed validation.");
            return View(premises);
        }

        _context.Add(premises);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Premises created {PremisesId} {Name}", premises.Id, premises.Name);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var premises = await _context.Premises.FindAsync(id);
        if (premises is null)
        {
            return NotFound();
        }

        return View(premises);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Town,RiskRating")] Premises premises)
    {
        if (id != premises.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Premises edit failed validation for {PremisesId}", premises.Id);
            return View(premises);
        }

        try
        {
            _context.Update(premises);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Premises updated {PremisesId} {Name}", premises.Id, premises.Name);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!PremisesExists(premises.Id))
            {
                return NotFound();
            }

            _logger.LogError(ex, "Premises update failed {PremisesId}", premises.Id);
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

        var premises = await _context.Premises.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (premises is null)
        {
            return NotFound();
        }

        return View(premises);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var premises = await _context.Premises.FindAsync(id);
        if (premises is null)
        {
            return NotFound();
        }

        _context.Premises.Remove(premises);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Premises deleted {PremisesId} {Name}", premises.Id, premises.Name);
        return RedirectToAction(nameof(Index));
    }

    private bool PremisesExists(int id)
    {
        return _context.Premises.Any(e => e.Id == id);
    }
}
