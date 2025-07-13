#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Security.Claims;
using Palesteeny_Project.Services;

namespace Palesteeny_Project.Controllers
{
    public class QuizsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> TopThreePartial()
        {
            var topThree = await _context.QuizResults
                .Include(r => r.User)
                .GroupBy(r => r.UserPalId)
                .Select(g => new
                {
                    User = g.First().User,
                    TotalScore = g.Sum(r => r.TotalScore)
                })
                .OrderByDescending(x => x.TotalScore)
                .Take(3)
                .ToListAsync();

            return PartialView("_TopThreePartial", topThree);
        }

        // ✅ عرض كل التصنيفات الموجودة فعلياً في الأسئلة
        // GET: /Quizs/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.QuizQuestions
                                           .Where(q => q.Category != null && q.Category != "")
                                           .Select(q => q.Category)
                                           .Distinct()
                                           .ToListAsync();

            return View(categories);
        }

        // GET: /Quizs/Index?page=1&category=جغرافيا
        public async Task<IActionResult> Index(int page = 1, string category = "جغرافيا")
        {
            int pageSize = 3;

            var query = _context.QuizQuestions
                                .Where(q => q.Category == category)
                                .Include(q => q.Options);

            int totalQuestions = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalQuestions / (double)pageSize);

            if (totalPages == 0)
                totalPages = 1;

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var questions = await query
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            ViewBag.SelectedCategory = category;

            return View(questions);
        }

        // POST: /Quizs/SubmitAnswers
        [HttpPost]
     
        public async Task<IActionResult> SubmitAnswers(int page, string category, Dictionary<int, int> answers)

        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                return Unauthorized("يجب تسجيل الدخول للإجابة على الأسئلة.");

            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null)
                return Unauthorized("مستخدم غير معروف.");

            int userId = int.Parse(userIdStr);
            int pageSize = 3;

            // استرجاع الإجابات السابقة من الـ Session أو إنشاء جديد
            Dictionary<int, int> answersDict = HttpContext.Session.GetObject<Dictionary<int, int>>("QuizAnswers") ?? new();

            var questions = await _context.QuizQuestions
                .Where(q => q.Category == category)
                .Include(q => q.Options)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // تحديث القاموس بالإجابات الجديدة من الصفحة الحالية
            foreach (var question in questions)
            {
                if (answers.TryGetValue(question.Id, out int selectedOptionId))
                {
                    if (selectedOptionId != 0)
                    {
                        answersDict[question.Id] = selectedOptionId;
                    }
                }
            }


            // حفظ القاموس المحدث بالـ Session
            HttpContext.Session.SetObject("QuizAnswers", answersDict);

            // إذا الصفحة ليست الأخيرة ننتقل للصفحة التالية
            int totalQuestions = await _context.QuizQuestions.Where(q => q.Category == category).CountAsync();
            int totalPages = (int)Math.Ceiling(totalQuestions / (double)pageSize);

            if (page < totalPages)
            {
                return RedirectToAction("Index", new { page = page + 1, category });
            }

            // حساب النتيجة النهائية بعد انتهاء كل الصفحات
            int totalScore = 0;
            foreach (var kvp in answersDict)
            {
                var question = await _context.QuizQuestions.Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == kvp.Key);
                if (question != null)
                {
                    var selectedOption = question.Options.FirstOrDefault(o => o.Id == kvp.Value);
                    if (selectedOption != null && selectedOption.IsCorrect)
                    {
                        totalScore += question.Score;
                    }
                }
            }

            var quizResult = new QuizResult
            {
                UserPalId = userId,
                TotalScore = totalScore,
                TakenAt = DateTime.Now,
                Category = category
            };

            _context.QuizResults.Add(quizResult);
            await _context.SaveChangesAsync();

            // مسح الإجابات المؤقتة بعد الحفظ
            HttpContext.Session.Remove("QuizAnswers");

            return RedirectToAction("QuizResult", new { resultId = quizResult.Id });
        }


        // GET: /Quizs/Result
        public async Task<IActionResult> Result(string category)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = null;

            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int parsedId))
            {
                userId = parsedId;
            }

            // حساب مجموع النقاط لكل مستخدم عبر جميع التصنيفات (Leaderboard)
            var leaderboard = await _context.QuizResults
                .GroupBy(r => r.UserPalId)
                .Select(g => new QuizResult
                {
                    UserPalId = g.Key,
                    TotalScore = g.Sum(r => r.TotalScore),
                    User = g.Select(r => r.User).FirstOrDefault()
                })
                .OrderByDescending(r => r.TotalScore)
                .Take(10)
                .ToListAsync();

            // الحصول على نتيجة المستخدم في هذا التصنيف فقط (آخر نتيجة له)
            QuizResult? userResult = null;
            if (userId.HasValue)
            {
                userResult = await _context.QuizResults
                    .Where(r => r.UserPalId == userId.Value && r.Category == category)
                    .OrderByDescending(r => r.TakenAt)
                    .FirstOrDefaultAsync();
            }

            ViewBag.UserResult = userResult;
            ViewBag.Leaderboard = leaderboard;
            ViewBag.SelectedCategory = category;

            return View();
        }

        public async Task<IActionResult> QuizResult(int resultId)
        {
            var result = await _context.QuizResults
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == resultId);

            if (result == null)
                return NotFound();

            return View(result); // سيعرض View جديد فيه نتيجة هذا الاختبار فقط
        }

    }
}