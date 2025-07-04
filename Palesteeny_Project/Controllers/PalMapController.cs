using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Threading.Tasks;

namespace Palesteeny_Project.Controllers
{
    public class PalMapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PalMapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // عرض أي مدينة حسب ID
        [HttpGet]
        public async Task<IActionResult> City(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null) return NotFound();
            return View("City", city);
        }

        // خريطة فلسطين
        public IActionResult PalMap() => View();
    }
}
