using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;

namespace Palesteeny_Project.ViewComponents
{
    public class AdBannerViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AdBannerViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var ad = _context.Ads
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefault();

            return View("Default", ad); // Views/Shared/Components/AdBanner/Default.cshtml
        }
    }
}
