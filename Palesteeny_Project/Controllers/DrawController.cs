using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using Palesteeny_Project.ViewModels;
using System.Text.RegularExpressions;

namespace Palesteeny_Project.Controllers
{
    public class DrawController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DrawController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // استخدم async هنا لتحسين الأداء
       public IActionResult Draw()
{
            var categories = _context.Templates
              .Select(t => t.Category)
              .Where(c => c != null)         // تأكد من إزالة null
              .Distinct()
              .Cast<string>()                // نحول من string? إلى string
              .ToList();

            var userId = HttpContext.Session.GetInt32("UserPalId"); // أو حسب طريقة تسجيل الدخول
            ViewBag.UserPalId = userId;
            var model = new DrawPageViewModel
    {
        Categories = categories
    };


    return View(model);

}





        [HttpPost]
        public async Task<IActionResult> ShareDrawing([FromBody] DrawingDto dto)
        {
            if (string.IsNullOrEmpty(dto.Base64Image))
                return BadRequest("Missing image");

            var fileName = $"drawing_{Guid.NewGuid()}.png";
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

            var base64Data = Regex.Match(dto.Base64Image, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var imageBytes = Convert.FromBase64String(base64Data);
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            var drawing = new Drawing
            {
                UserPalId = dto.UserPalId,
                ImageUrl = $"/uploads/{fileName}",
                Mode = dto.Mode,
                TemplateId = dto.Mode == "template" ? dto.TemplateId : null,
               
            };

            _context.Drawings.Add(drawing);
            await _context.SaveChangesAsync();

            return Ok();
        }


        public IActionResult Gallery()
        {
            var drawings = _context.Drawings
                .Include(d => d.User)
                .Include(d => d.Template)
                .OrderByDescending(d => d.DrawingId)
                .ToList();

            return View(drawings);
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Templates
                .Select(t => t.Category)
                .Distinct()
                .ToList();

            return Json(categories);
        }
        [HttpGet]
        public IActionResult GetTemplatesByCategory(string category)
        {
            var templates = _context.Templates
                .Where(t => t.Category == category)
                .Select(t => new {
                    id = t.Id,
                    imageUrl = t.ImageUrl,
                    category = t.Category
                })
                .ToList();

            return Json(templates);
        }


    }
}
