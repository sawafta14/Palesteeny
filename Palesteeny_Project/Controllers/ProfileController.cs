using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return NotFound("المستخدم غير موجود.");

            return View(user); // Pass user data to the view
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
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null) return NotFound();

            return Json(new
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                grade = user.Grade,
                semester = user.Semester,
                imagePath = user.ImagePath
            });
        }

        // POST: /Profile/UpdateProfile
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null) return NotFound();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Grade = dto.Grade;
            user.Semester = dto.Semester;
            user.Age = GetAgeFromGrade(dto.Grade);

            // Only update image path if provided
            if (!string.IsNullOrEmpty(dto.ImagePath))
                user.ImagePath = dto.ImagePath;

            await _context.SaveChangesAsync();
            return Ok(new { message = "تم التحديث بنجاح" });
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Grade { get; set; }
        public string Semester { get; set; }
        public string ImagePath { get; set; }
    }
}
