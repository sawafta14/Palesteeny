using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;
using Palesteeny_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Palesteeny_Project.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Palesteeny_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: عرض صفحة التسجيل
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var semesters = await _context.Semesters.ToListAsync();
            var model = new RegisterViewModel
            {
                SemesterSelectList = semesters.Select(s => new SelectListItem
                {
                    Value = s.SemesterId.ToString(),
                    Text = $"{s.GradeName} - {s.SemesterName}"
                })
            };
            return View(model);
        }


        // POST: استقبال بيانات التسجيل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userExists = await _context.UsersPal.AnyAsync(u => u.Email == model.Email);
            if (userExists)
            {
                ModelState.AddModelError(nameof(model.Email), "هذا البريد مسجل مسبقًا.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<UserPal>();

            var user = new UserPal
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Gender = model.Gender,
                SemesterId = model.SemesterId, // تأكد أن هذا الحقل موجود في نموذج التسجيل (model)
                PasswordHash = passwordHasher.HashPassword(null!, model.Password),
                EmailConfirmed = false,
                ConfirmationToken = Guid.NewGuid().ToString()
            };


            _context.UsersPal.Add(user);
            await _context.SaveChangesAsync();

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { email = user.Email, token = user.ConfirmationToken }, Request.Scheme);

            string emailBody = $@"
                <h3>مرحبًا {user.FirstName},</h3>
                <p>شكرًا لتسجيلك. الرجاء تأكيد بريدك الإلكتروني بالضغط على الرابط التالي:</p>
                <a href='{confirmationLink}'>تأكيد البريد الإلكتروني</a>";

            await _emailSender.SendEmailAsync(user.Email, "تأكيد البريد الإلكتروني", emailBody);

            TempData["SuccessMessage"] = "تم التسجيل بنجاح. يرجى التحقق من بريدك الإلكتروني.";
            return RedirectToAction("ConfirmEmailRequest");
        }

        // GET: تأكيد البريد الإلكتروني من الرابط
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("البيانات غير صحيحة.");

            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("المستخدم غير موجود.");

            if (user.EmailConfirmed)
                return Content("تم تأكيد البريد الإلكتروني سابقًا.");

            if (user.ConfirmationToken != token)
                return BadRequest("رمز التأكيد غير صحيح.");

            user.EmailConfirmed = true;
            user.ConfirmationToken = null;
            await _context.SaveChangesAsync();

            return View("Login");
        }

        // GET: عرض نموذج لإعادة إرسال رابط التأكيد
        [HttpGet]
        public IActionResult ConfirmEmailRequest()
        {
            return View();
        }

        // POST: إرسال رابط تأكيد جديد إلى البريد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmailRequest(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "المستخدم غير موجود.");
                return View(model);
            }

            if (user.EmailConfirmed)
            {
                TempData["InfoMessage"] = "تم تأكيد البريد الإلكتروني سابقًا.";
                return RedirectToAction("Login");
            }

            // إعادة توليد رمز التأكيد (اختياري)
            user.ConfirmationToken = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { email = user.Email, token = user.ConfirmationToken }, Request.Scheme);

            string emailBody = $@"
                <h3>مرحبًا {user.FirstName},</h3>
                <p>الرجاء تأكيد بريدك الإلكتروني بالضغط على الرابط التالي:</p>
                <a href='{confirmationLink}'>تأكيد البريد الإلكتروني</a>";

            await _emailSender.SendEmailAsync(user.Email, "تأكيد البريد الإلكتروني", emailBody);

            TempData["SuccessMessage"] = "تم إرسال رابط تأكيد جديد إلى بريدك الإلكتروني.";
            return RedirectToAction("Login");
        }

        // GET: صفحة تسجيل الدخول
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: تسجيل الدخول
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "يجب تأكيد البريد الإلكتروني أولاً.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<UserPal>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
                return View(model);
            }

       var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim("Role", user.Role),

    new Claim("Grade", user.Semester?.GradeName ?? "Unknown"), // إذا حذفت Grade من user
    new Claim("Gender", user.Gender)


};


            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserGrade", user.Semester?.GradeName ?? "Unknown");
            HttpContext.Session.SetString("UserGender", user.Gender);


            return RedirectToAction("Index", "Home");
        }

        // POST: تسجيل الخروج
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        // GET: عرض صفحة تغيير كلمة المرور
        [HttpGet]
        public IActionResult ChangePassword()
        {
           

            return View();
        }

        // POST: استقبال تغيير كلمة المرور
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), "المستخدم غير موجود.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<UserPal>();
            user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);

            await _context.SaveChangesAsync();

            // بعد تغيير كلمة المرور، ننتقل إلى صفحة تأكيد البريد
            return RedirectToAction("ConfirmEmailRequest");
        }

    }
}
