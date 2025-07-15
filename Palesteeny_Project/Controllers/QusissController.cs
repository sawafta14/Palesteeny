using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace Palesteeny_Project.Controllers
{
    public class QusissController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QusissController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Qusis()
        {
            var qusis = await _context.Qusis
                .Include(q => q.Images.OrderBy(i => i.Order))
                .ToListAsync();

            var categories = qusis
                .Select(q => q.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToList();

            ViewData["Categories"] = categories;

            return View(qusis);
        }

        public async Task<IActionResult> Qusa(int id)
        {
            var qusi = await _context.Qusis
                .Include(q => q.Images.OrderBy(i => i.Order))
                .FirstOrDefaultAsync(q => q.Id == id);

            if (qusi == null)
                return NotFound();

            int? userId = GetCurrentUserId();
            int bookmarkOrder = 0;
            bool isFavorite = false;

            if (userId != null)
            {
                var bookmark = await _context.StoryBookmarks
                    .FirstOrDefaultAsync(b => b.UserPalId == userId && b.QusId == id);

                if (bookmark != null)
                    bookmarkOrder = bookmark.LastImageIndex;

                isFavorite = await _context.FavoriteStories
                    .AnyAsync(f => f.UserPalId == userId && f.QusId == id);
            }

            var model = new QusaViewModel
            {
                Qusi = qusi,
                BookmarkOrder = bookmarkOrder,
                IsFavorite = isFavorite,
                UserId = userId
            };

            return View(model);
        }

        public async Task<IActionResult> Favorite()
        {
            int? userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var favorites = await _context.FavoriteStories
                .Include(f => f.Qusi)
                .Where(f => f.UserPalId == userId)
                .Select(f => f.Qusi!)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int qusiId)
        {
            int? userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();
          

            // تحقق من وجود المستخدم
            var userExists = await _context.UsersPal.AnyAsync(u => u.Id == userId.Value);
            if (!userExists)
                return BadRequest("المستخدم غير موجود.");


            // ✅ تحقق من أن القصة موجودة
            var exists = await _context.Qusis.AnyAsync(q => q.Id == qusiId);
            if (!exists)
                return NotFound("القصة غير موجودة");

            var favorite = await _context.FavoriteStories
                .FirstOrDefaultAsync(f => f.UserPalId == userId && f.QusId == qusiId);

            if (favorite == null)
            {
                _context.FavoriteStories.Add(new FavoriteStory
                {
                    UserPalId = userId.Value,
                    QusId = qusiId
                });
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }

                return Json(new { isFavorite = true });
            }
            else
            {
                _context.FavoriteStories.Remove(favorite);
                await _context.SaveChangesAsync();
                return Json(new { isFavorite = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveBookmark(int qusiId, int imageOrder)
        {
            int? userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var qusiExists = await _context.Qusis.AnyAsync(q => q.Id == qusiId);
            if (!qusiExists)
                return BadRequest("القصة غير موجودة.");

            // تحقق من وجود المستخدم
            var userExists = await _context.UsersPal.AnyAsync(u => u.Id == userId.Value);
            if (!userExists)
                return BadRequest("المستخدم غير موجود.");


            var bookmark = await _context.StoryBookmarks
                .FirstOrDefaultAsync(b => b.UserPalId == userId && b.QusId == qusiId);

            if (bookmark == null)
            {
                bookmark = new StoryBookmark
                {
                    UserPalId = userId.Value,
                    QusId = qusiId,
                    LastImageIndex = imageOrder
                };
                _context.StoryBookmarks.Add(bookmark);
            }
            else
            {
                bookmark.LastImageIndex = imageOrder;
                _context.StoryBookmarks.Update(bookmark);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            return Json(new { success = true });
        }

        private int? GetCurrentUserId()
        {
            if (User.Identity?.IsAuthenticated != true)
                return null;

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out int userId) ? userId : null;
        }
    }

    public class QusaViewModel
    {
        public Qusi Qusi { get; set; } = null!;
        public int BookmarkOrder { get; set; }
        public bool IsFavorite { get; set; }
        public int? UserId { get; set; }
    }
}
