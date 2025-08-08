using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Data;
using PersonalAccounting.Models;

namespace PersonalAccounting.Controllers;

public class ChartsController : Controller
{
    private readonly ApplicationDbContext _db;

    public ChartsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> MonthlySummary(int year)
    {
        if (year <= 0) year = DateTime.Now.Year;

        var data = await _db.Transactions
            .Where(t => t.Date.Year == year)
            .GroupBy(t => new { t.Date.Year, t.Date.Month, t.Type })
            .Select(g => new
            {
                Month = g.Key.Month,
                Type = g.Key.Type,
                Total = g.Sum(x => x.Amount)
            })
            .ToListAsync();

        var income = new decimal[12];
        var expense = new decimal[12];
        foreach (var d in data)
        {
            if (d.Type == TransactionType.Income) income[d.Month - 1] = d.Total;
            else expense[d.Month - 1] = d.Total;
        }
        return Json(new { income, expense });
    }
}