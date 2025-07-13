using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;

public class Admins0Controller : Controller
{
    public IActionResult Index()
    {
        var role = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        if (role != "admin")
        {
             RedirectToAction("Index", "Home");
        }

        return View();
    }
}
