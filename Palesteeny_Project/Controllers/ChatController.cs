using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models; // For ChatMessage and UserPal
using Microsoft.EntityFrameworkCore; // For async queries
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Claims;

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
            return RedirectToAction("Index"); // e.g., "Index", "Lesson1", etc.

        }
        [HttpPost]
        public async Task<IActionResult> AiAssistant(ChatMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.UserMessage))
            {
                ViewBag.Reply = "يرجى إدخال رسالة.";
                return PartialView("_AiReply");
            }

            var currentUserEmail = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _context.UsersPal.FirstOrDefaultAsync(u => u.Email == currentUserEmail);
            var userName = string.IsNullOrWhiteSpace(user?.FirstName) ? "الصديق العزيز" : user.FirstName;

            var promptText = $@"تحدث كمساعد ذكي مخصص للأطفال، وأجب على السؤال التالي بأسلوب ممتع، بسيط، ولطيف لطفل صغير.
                        اسم الطفل هو {userName}.\n\n{message.UserMessage}";

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
            return PartialView("_AiReply"); // ✅ returns only the reply's HTML
        }


    }
}
