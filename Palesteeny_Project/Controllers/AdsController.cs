using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;

namespace Palesteeny_Project.Controllers
{
    public class AdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult LoadAd()
        {
            // جلب أول إعلان مفعّل فقط
            var ad = _context.Ads
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefault();

            return PartialView("_AdPartial", ad);
        }
    }
}
