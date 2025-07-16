using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using Palesteeny_Project.Models;
using System.Threading.Tasks;

namespace Palesteeny_Project.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly string apiKey = "AIzaSyAVlHYjYjqylNmX8y68Ai7l4jtLWp587ZA";
        private readonly string endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=";

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AiAssistant()
        {
            return RedirectToAction("Index");
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



        [HttpPost]
        public async Task<IActionResult> AiAssistant(ChatMessage message)
        {
            var contents = await _context.SearchableContents.ToListAsync();
            if (string.IsNullOrWhiteSpace(message.UserMessage))
            {
                ViewBag.Reply = "يرجى إدخال رسالة.";
                return PartialView("_AiReply");
            }

            var currentUserEmail = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _context.UsersPal
                .Include(u => u.PreferredAssistant)
                .Include(u => u.Semester)
                .FirstOrDefaultAsync(u => u.Email == currentUserEmail);

            var userName = string.IsNullOrWhiteSpace(user?.FirstName) ? "الصديق العزيز" : user.FirstName;

            var assistantGender = user?.PreferredAssistant?.Gender?.Trim();
            var assistantName = user?.PreferredAssistant?.Name?.Trim();

            var childGender = user?.Gender?.Trim(); // 👈 make sure this exists in your model

            // Assistant info
            var characterPronoun = assistantGender == "أنثى" ? "صديقتكِ" : "صديقك";
            var characterAdj = assistantGender == "أنثى" ? "مساعدة ذكية" : "مساعد ذكي";

            // Get the matched page once
            var matchedPage = await GetPageFromSearchableContent(message.CurrentPage);
            string friendlyPageName = matchedPage?.Title ?? message.CurrentPage ?? "غير معروف";

            var promptText = $@"أنت اسمك {assistantName}، وجنسك هو {(assistantGender ?? "غير معروف")}، .
        ابدأ رسالتك بجملة: ""مرحباً، أنا {assistantName}، {characterPronoun} !"" 

        اسم الطفل هو {userName}. وجنسه: {childGender ?? "غير معروف"}.
        الصف: {user?.Semester?.GradeName}، الفصل: {user?.Semester?.SemesterName}.
        الموقع الحالي في المنصة هو: {friendlyPageName}.
        السؤال:
        {message.UserMessage}

        أجب بطريقة مشجعة، لطيفة، ومناسبة للأطفال.";

            var requestBody = new
            {
                contents = new[] {
            new {
                role = "user",
                parts = new[] {
                    new { text = promptText }
                }
            }
        }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await new HttpClient().PostAsync($"{endpoint}{apiKey}", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Reply = "حدث خطأ أثناء الاتصال بالمساعد.";
                return PartialView("_AiReply");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var reply = doc.RootElement
                           .GetProperty("candidates")[0]
                           .GetProperty("content")
                           .GetProperty("parts")[0]
                           .GetProperty("text")
                           .GetString();

            ViewBag.Reply = reply;

            ViewBag.PreferredAssistantImage = user?.PreferredAssistant?.ImageUrl ?? $"/images/{user?.PreferredAssistant?.Color}Tall{(user?.PreferredAssistant?.Gender == "male" ? "Teeny" : "Teena")}.svg";


            if (user != null)
            {
                var log = new ChatLog
                {
                    UserPalId = user.Id,
                    Message = message.UserMessage!,
                    Reply = reply,
                    PageName = matchedPage?.Title ?? message.CurrentPage,
                    PageId = matchedPage?.Id,
                    Timestamp = DateTime.Now
                };

                _context.ChatLogs.Add(log);
                await _context.SaveChangesAsync();
            }

            return PartialView("_AiReply");
        }


        // Optional mapping: you can expand logic later
        private async Task<SearchableContent?> GetPageFromSearchableContent(string? page)
        {
            if (string.IsNullOrWhiteSpace(page))
                return null;

            var lowerPage = page.ToLower().Trim();
            var contents = await _context.SearchableContents.ToListAsync();

            Console.WriteLine("🔎 Matching page for: " + lowerPage);

            foreach (var c in contents)
            {
                var dbUrl = c.Url?.ToLower().Trim();
                if (!string.IsNullOrWhiteSpace(dbUrl))
                {
                    Console.WriteLine($"➡️ Trying DB URL: '{dbUrl}'");
                    if (lowerPage.Contains(dbUrl))
                    {
                        Console.WriteLine("✅ Matched: " + dbUrl);
                        return c;
                    }
                }
            }

            Console.WriteLine("❌ No match found.");
            return null;
        }


    }
}
