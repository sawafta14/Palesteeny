using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        var role = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        if (role != "admin")
        {
            return Unauthorized(); // أو: RedirectToAction("Index", "Home");
        }

        return View();
    }
}
