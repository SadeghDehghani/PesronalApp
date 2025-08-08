using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Data;
using PersonalAccounting.Models;

namespace PersonalAccounting.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _db;

    public CategoriesController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
        return View(list);
    }

    public IActionResult Create()
    {
        return PartialView("_CreateOrEdit", new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("_CreateOrEdit", model);
        }
        _db.Categories.Add(model);
        await _db.SaveChangesAsync();
        return Json(new { success = true });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _db.Categories.FindAsync(id);
        if (model == null) return NotFound();
        return PartialView("_CreateOrEdit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return PartialView("_CreateOrEdit", model);
        _db.Update(model);
        await _db.SaveChangesAsync();
        return Json(new { success = true });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var model = await _db.Categories.FindAsync(id);
        if (model == null) return NotFound();
        return PartialView("_Delete", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _db.Categories.FindAsync(id);
        if (model == null) return NotFound();
        _db.Categories.Remove(model);
        await _db.SaveChangesAsync();
        return Json(new { success = true });
    }
}