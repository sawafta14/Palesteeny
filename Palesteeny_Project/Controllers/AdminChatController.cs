using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Linq;

namespace Palesteeny_Project.Controllers
{
    public class AdminChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public async Task<IActionResult> Manage()
        {
            // 1. Top active users
            var activeUsers = await _context.ChatLogs
                .Include(c => c.User)
                .GroupBy(c => c.UserPalId)
                .Select(g => new
                {
                    UserId = g.Key,
                    UserName = g.First().User!.FirstName,



                    MessageCount = g.Count(),
                    LastMessage = g.Max(c => c.Timestamp)
                })
                .OrderByDescending(x => x.MessageCount)
                .Take(5)
                .ToListAsync();

            // 2. Top pages or topics
            var popularPages = await _context.ChatLogs
                .GroupBy(c => c.PageName)
                .Select(g => new
                {
                    PageName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(p => p.Count)
                .ToListAsync();

            ViewBag.ActiveUsers = activeUsers;
            ViewBag.PopularPages = popularPages;

            return View();
        }
    }
}
