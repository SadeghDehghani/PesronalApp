using Microsoft.AspNetCore.Mvc;

namespace PersonalAccounting.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}