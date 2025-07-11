using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

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
                .Include(q => q.Images.OrderBy(i => i.Order)) // جلب الصور مرتبة
                .ToListAsync();

            // استخراج التصنيفات الفريدة
            var categories = qusis
                .Select(q => q.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToList();

            ViewData["Categories"] = categories;

            return View(qusis);
        }

        // 2. عرض قصة واحدة مع دعم البوك مارك
        public async Task<IActionResult> Qusa(int id)
        {
            var qusi = await _context.Qusis
                .Include(q => q.Images.OrderBy(i => i.Order))
                .FirstOrDefaultAsync(q => q.Id == id);

            if (qusi == null)
                return NotFound();

            int? userId = GetCurrentUserId();

            int bookmarkOrder = 0; // رقم الصورة للبوك مارك، 0 إذا لا يوجد
            bool isFavorite = false;

            if (userId != null)
            {
                var bookmark = await _context.StoryBookmarks
                    .FirstOrDefaultAsync(b => b.UserPalId == userId && b.QusiId == id);

                if (bookmark != null)
                    bookmarkOrder = bookmark.LastImageIndex;

                isFavorite = await _context.FavoriteStories
                    .AnyAsync(f => f.UserPalId == userId && f.QusiId == id);
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

        // 3. صفحة المفضلة (قصص أضافها المستخدم كمفضلة)
        public async Task<IActionResult> Favorite()
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var favorites = await _context.FavoriteStories
                .Include(f => f.Qusi)
                .Where(f => f.UserPalId == userId)
                .Select(f => f.Qusi)
                .ToListAsync();

            return View(favorites);
        }

        // 4. إضافة أو حذف قصة من المفضلة (AJAX)
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int qusiId)
        {
            int? userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var favorite = await _context.FavoriteStories
                .FirstOrDefaultAsync(f => f.UserPalId == userId && f.QusiId == qusiId);

            if (favorite == null)
            {
                _context.FavoriteStories.Add(new FavoriteStory
                {
                    UserPalId = userId.Value,
                    QusiId = qusiId
                });
                await _context.SaveChangesAsync();
                return Json(new { isFavorite = true });
            }
            else
            {
                _context.FavoriteStories.Remove(favorite);
                await _context.SaveChangesAsync();
                return Json(new { isFavorite = false });
            }
        }

        // 5. حفظ البوك مارك (مكان توقف المستخدم في الصور)
        [HttpPost]
        public async Task<IActionResult> SaveBookmark(int qusiId, int imageOrder)
        {
            int? userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var bookmark = await _context.StoryBookmarks
                .FirstOrDefaultAsync(b => b.UserPalId == userId && b.QusiId == qusiId);

            if (bookmark == null)
            {
                bookmark = new StoryBookmark
                {
                    UserPalId = userId.Value,
                    QusiId = qusiId,
                    LastImageIndex = imageOrder
                };
                _context.StoryBookmarks.Add(bookmark);
            }
            else
            {
                bookmark.LastImageIndex = imageOrder;
                _context.StoryBookmarks.Update(bookmark);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // 6. مساعدة للحصول على معرف المستخدم الحالي من الـ Claims
        private int? GetCurrentUserId()
        {
            if (User.Identity?.IsAuthenticated != true)
                return null;

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out int userId) ? userId : null;
        }
    }

    // ViewModel لعرض قصة واحدة مع البوك مارك والمفضلة
    public class QusaViewModel
    {
        public Qusi Qusi { get; set; } = null!;
        public int BookmarkOrder { get; set; }
        public bool IsFavorite { get; set; }
        public int? UserId { get; set; }
    }
}
