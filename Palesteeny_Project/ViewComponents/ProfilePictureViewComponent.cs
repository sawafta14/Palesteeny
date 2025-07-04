using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;       // Your DbContext and models namespace
using System.Security.Claims;
using System.Linq;

namespace Palesteeny_Project.ViewComponents
{
    public class ProfilePictureViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProfilePictureViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            string? userEmail = claimsPrincipal?.FindFirst(ClaimTypes.Email)?.Value;

            string imagePath = "~/images/profileDef.png"; // default image

            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = _context.UsersPal.FirstOrDefault(u => u.Email == userEmail);
                if (user != null && !string.IsNullOrEmpty(user.ImagePath))
                {
                    imagePath = user.ImagePath;
                }
            }

            return View("Default", imagePath);
        }
    }
}
