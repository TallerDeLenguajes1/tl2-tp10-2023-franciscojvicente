using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;
using tl2_tp10_2023_franciscojvicente.ViewModel;

namespace tl2_tp10_2023_franciscojvicente.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // readonly UsuarioRepository repositorioUser = new();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        string? errorMessage = TempData["ErrorMessage"] as string;
        string? stackTrace = TempData["StackTrace"] as string;
        

        // Puedes pasar los datos a la vista si es necesario
        ViewData["ErrorMessage"] = errorMessage;
        ViewData["StackTrace"] = stackTrace;

        return View();
        // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
