using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string? category)
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
