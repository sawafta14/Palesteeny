using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;

namespace Palesteeny_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult game()
        {
            return View();
        }
        public IActionResult about()
        {
            return View();
        }
        public IActionResult Tasali()
        {
            return View();
        }

      
        public IActionResult Slider()
        {
            return View();
        }
        public IActionResult BunnyGlasses()
        {
            return View();
        }
      
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult puzzleweb()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
