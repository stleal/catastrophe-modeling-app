using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace catastrophe_modeling_app_mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
