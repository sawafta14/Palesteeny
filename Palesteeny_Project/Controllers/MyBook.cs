using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using Palesteeny_Project.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Palesteeny_Project.Controllers
{
    [Authorize]
    public class MyBookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyBookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /MyBook/Ketabi
        public async Task<IActionResult> Ketabi()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (!int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized(); // or handle error
            }

            var userPal = await _context.UsersPal
                .Include(u => u.Semester)
                .FirstOrDefaultAsync(u => u.Id == userId);



            if (userPal == null || userPal.Semester == null)
                return NotFound("User profile or semester not found.");

            // Get the matching Book for the user's semester and grade
            var book = await _context.Books
              .Include(b => b.Lessons)
              .FirstOrDefaultAsync(b => b.SemesterId == userPal.SemesterId);
            // After loading Lessons
            var lessons = book?.Lessons.OrderBy(l => l.StartPage).ToList() ?? new List<Lesson>();

            foreach (var lesson in lessons)
            {
                if (!string.IsNullOrEmpty(lesson.VideoUrl))
                {
                    lesson.VideoUrl = lesson.VideoUrl.Trim().Replace("\r", "").Replace("\n", "");
                }
            }

            var viewModel = new KetabiViewModel
            {
                PdfUrl = book?.PdfUrl ?? string.Empty,
                UserPalId = userPal.Id,
                SemesterId = userPal.SemesterId,
                Lessons = lessons
            };

            return View(viewModel);
        }
        [HttpPost]
        [HttpPost]
        [IgnoreAntiforgeryToken] // Optional if no CSRF token is used
        public async Task<IActionResult> SaveBookmark([FromBody] BookmarkRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var userPal = await _context.UsersPal.FirstOrDefaultAsync(u => u.Id == userId);
            if (userPal == null)
                return NotFound("User not found.");

            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonId == request.LessonId);
            if (lesson == null)
                return NotFound("Lesson not found.");

            var userLesson = await _context.UserLessons
                .FirstOrDefaultAsync(ul => ul.UserPalId == userId && ul.LessonId == request.LessonId);

            if (userLesson == null)
            {
                userLesson = new UserLesson
                {
                    UserPalId = userId,
                    LessonId = request.LessonId,
                    BookmarkPage = lesson.StartPage
                };
                _context.UserLessons.Add(userLesson);
            }
            else
            {
                userLesson.BookmarkPage = lesson.StartPage;
                _context.UserLessons.Update(userLesson);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetBookmarkPage()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            var userLesson = await _context.UserLessons
                .Where(ul => ul.UserPalId == userId && ul.BookmarkPage != null)
                .OrderByDescending(ul => ul.Id)
                .FirstOrDefaultAsync();

            if (userLesson == null)
                return Json(new { success = false });

            return Json(new { success = true, page = userLesson.BookmarkPage });
        }

    }
}

