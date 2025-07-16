#nullable enable

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Palesteeny_Project.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Profile/Hesabi
        public async Task<IActionResult> Hesabi()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal
                .Include(u => u.Semester) // ✅ This is what was missing!
                .Include(u => u.PreferredAssistant)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return NotFound("المستخدم غير موجود.");

            if (user.PreferredAssistant != null)
            {
                var suffix = user.PreferredAssistant.Gender == "male" ? "Teeny" : "Teena";
                ViewBag.PreferredAssistantImage = $"/images/{user.PreferredAssistant.Color}Tall{suffix}.svg";
            }
            else
            {
                ViewBag.PreferredAssistantImage = "/images/BlueTallTeeny.svg";
            }

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> LoadAssistant()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await _context.UsersPal
                .Include(u => u.PreferredAssistant)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null || user.PreferredAssistant == null)
            {
                return Json(new { aiImage = "/images/BlueTallTeeny.svg" });
            }

            var image = user.PreferredAssistant.ImageUrl
                ?? $"/images/{user.PreferredAssistant.Color}Tall{(user.PreferredAssistant.Gender == "male" ? "Teeny" : "Teena")}.svg";

            return Json(new { aiImage = image });
        }


        // POST: /Profile/UploadProfileImage
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("يرجى اختيار صورة.");

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return NotFound();

            // Save uploaded image
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsPath); // Ensure directory exists

            var filePath = Path.Combine(uploadsPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Update user's image path in the database
            user.ImagePath = $"/uploads/{fileName}";
            await _context.SaveChangesAsync();

            return Ok(new { imageUrl = user.ImagePath });
        }


        // GET: /Profile/GetUserInfo
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal
                .Include(u => u.Semester)
                .Include(u => u.PreferredAssistant)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null) return NotFound();

            var aiGender = user.PreferredAssistant?.Gender ?? "male";
            var aiColor = user.PreferredAssistant?.Color ?? "Green";
            var imageSuffix = aiGender == "male" ? "Teeny" : "Teena";
            var fallbackImage = $"/images/{aiColor}Tall{imageSuffix}.svg";

            return Json(new
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                semester = new
                {
                    gradeName = user.Semester?.GradeName,
                    semesterName = user.Semester?.SemesterName
                },
                imagePath = user.ImagePath,
                aiColor = aiColor,
                aiImage = user.PreferredAssistant?.ImageUrl ?? fallbackImage,
                aiGender = aiGender
            });
        }




        // POST: /Profile/UpdateProfile
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == userEmail);

           

            if (user == null)
            {
                
                return NotFound();
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;


            user.SemesterId = dto.SemesterId;

            if (!string.IsNullOrEmpty(dto.ImagePath))
                user.ImagePath = dto.ImagePath;

            await _context.SaveChangesAsync();
            return Ok(new { message = "تم التحديث بنجاح" });
        }
        [HttpGet]
        public async Task<IActionResult> GetSemesters()
        {
            var semesters = await _context.Semesters.ToListAsync();
            return Json(semesters.Select(s => new {
                id = s.SemesterId,
                gradeName = s.GradeName,
                semesterName = s.SemesterName
            }));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssistantPreference([FromBody] AssistantPreferenceDto dto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal.Include(u => u.PreferredAssistant)
                                              .FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return NotFound();

            var assistant = await _context.AIAssistant
                .FirstOrDefaultAsync(a => a.Gender == dto.Gender && a.Color == dto.Color);

            if (assistant == null)
            {
                return BadRequest("المساعد غير موجود.");
            }

            user.PreferredAssistant = assistant;
            await _context.SaveChangesAsync();

            return Ok(new { message = "تم حفظ تفضيل المساعد!" });
        }

        private int GetAgeFromGrade(string grade)
        {
            return grade switch
            {
                "الأول ابتدائي" => 6,
                "الثاني ابتدائي" => 7,
                "الثالث ابتدائي" => 8,
                "الرابع ابتدائي" => 9,
                _ => 0
            };
        }
    }

    public class UpdateProfileDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int SemesterId { get; set; }
        public string? ImagePath { get; set; }
    }
    public class AssistantPreferenceDto
    {
        public string? Gender { get; set; }
        public string? Color { get; set; }
    }

}
