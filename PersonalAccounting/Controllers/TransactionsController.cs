using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Data;
using PersonalAccounting.Models;
using PersonalAccounting.ViewModels;
using PersonalAccounting.Utils;

namespace PersonalAccounting.Controllers;

public class TransactionsController : Controller
{
    private readonly ApplicationDbContext _db;

    public TransactionsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(TransactionType? type, int? categoryId, string? fromPersianDate, string? toPersianDate)
    {
        var query = _db.Transactions.Include(t => t.Category).AsQueryable();

        if (type.HasValue) query = query.Where(t => t.Type == type);
        if (categoryId.HasValue) query = query.Where(t => t.CategoryId == categoryId);
        if (!string.IsNullOrWhiteSpace(fromPersianDate))
        {
            var from = DateTimeExtensions.FromShamsi(fromPersianDate);
            query = query.Where(t => t.Date >= from);
        }
        if (!string.IsNullOrWhiteSpace(toPersianDate))
        {
            var to = DateTimeExtensions.FromShamsi(toPersianDate).AddDays(1).AddTicks(-1);
            query = query.Where(t => t.Date <= to);
        }

        var items = await query.OrderByDescending(t => t.Date).ToListAsync();
        var vm = items.Select(t => new TransactionViewModel
        {
            Id = t.Id,
            Description = t.Description,
            Type = t.Type,
            Amount = t.Amount,
            PersianDate = t.Date.ToShamsiDate(),
            CategoryId = t.CategoryId,
            CategoryName = t.Category != null ? t.Category.Name : null
        }).ToList();

        ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
        return PartialView("_CreateOrEdit", new TransactionViewModel { Type = TransactionType.Expense, PersianDate = DateTime.Now.ToShamsiDate() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
            return PartialView("_CreateOrEdit", model);
        }

        var entity = new Transaction
        {
            Description = model.Description,
            Type = model.Type,
            Amount = model.Amount,
            Date = DateTimeExtensions.FromShamsi(model.PersianDate),
            CategoryId = model.CategoryId
        };
        _db.Transactions.Add(entity);
        await _db.SaveChangesAsync();
        return Json(new { success = true });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var t = await _db.Transactions.FindAsync(id);
        if (t == null) return NotFound();
        var vm = new TransactionViewModel
        {
            Id = t.Id,
            Description = t.Description,
            Type = t.Type,
            Amount = t.Amount,
            PersianDate = t.Date.ToShamsiDate(),
            CategoryId = t.CategoryId
        };
        ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
        return PartialView("_CreateOrEdit", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TransactionViewModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
            return PartialView("_CreateOrEdit", model);
        }
        var t = await _db.Transactions.FindAsync(id);
        if (t == null) return NotFound();
        t.Description = model.Description;
        t.Type = model.Type;
        t.Amount = model.Amount;
        t.Date = DateTimeExtensions.FromShamsi(model.PersianDate);
        t.CategoryId = model.CategoryId;
        await _db.SaveChangesAsync();
        return Json(new { success = true });
    }

    public async Task<IActionResult> Details(int id)
    {
        var t = await _db.Transactions.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (t == null) return NotFound();
        var vm = new TransactionViewModel
        {
            Id = t.Id,
            Description = t.Description,
            Type = t.Type,
            Amount = t.Amount,
            PersianDate = t.Date.ToShamsiDate(),
            CategoryId = t.CategoryId,
            CategoryName = t.Category?.Name
        };
        return PartialView("_Details", vm);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Transactions.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        if (t == null) return NotFound();
        var vm = new TransactionViewModel
        {
            Id = t.Id,
            Description = t.Description,
            Type = t.Type,
            Amount = t.Amount,
            PersianDate = t.Date.ToShamsiDate(),
            CategoryId = t.CategoryId,
            CategoryName = t.Category?.Name
        };
        return PartialView("_Delete", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var t = await _db.Transactions.FindAsync(id);
        if (t == null) return NotFound();
        _db.Transactions.Remove(t);
        await _db.SaveChangesAsync();
        return Json(new { success = true });
    }
}