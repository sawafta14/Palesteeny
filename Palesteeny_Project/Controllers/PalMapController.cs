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
            var City = await _context.Cities.FindAsync(id);
            if (City == null) return NotFound();
            return View("City", City);
        }

        // خريطة فلسطين
        public IActionResult PalMap() => View();
    }
}
