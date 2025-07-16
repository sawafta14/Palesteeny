using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Palesteeny_Project.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            _context.UsersPal.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Logout", "Account"); // or show a goodbye message
        }
    }
}
