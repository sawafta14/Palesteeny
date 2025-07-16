using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;

namespace Palesteeny_Project.Controllers
{
    public class ChatLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatLogsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("اضافه تخصيص جديد للمساعد", "Create", "ChatLogs"),
        ("التعديل على البيانات", "Index", "ChatLogs"),
        ("حذف ", "Index", "ChatLogs"),
        ("عرض البيانات", "Index", "ChatLogs")
    };

            return View(adminActions);
        }

        // GET: ChatLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChatLogs.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChatLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLog = await _context.ChatLogs
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatLog == null)
            {
                return NotFound();
            }

            return View(chatLog);
        }

        // GET: ChatLogs/Create
        public IActionResult Create()
        {
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email");
            return View();
        }

        // POST: ChatLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserPalId,Message,Reply,Timestamp,PageName,PageId")] ChatLog chatLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", chatLog.UserPalId);
            return View(chatLog);
        }

        // GET: ChatLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLog = await _context.ChatLogs.FindAsync(id);
            if (chatLog == null)
            {
                return NotFound();
            }
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", chatLog.UserPalId);
            return View(chatLog);
        }

        // POST: ChatLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserPalId,Message,Reply,Timestamp,PageName,PageId")] ChatLog chatLog)
        {
            if (id != chatLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatLogExists(chatLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", chatLog.UserPalId);
            return View(chatLog);
        }

        // GET: ChatLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatLog = await _context.ChatLogs
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatLog == null)
            {
                return NotFound();
            }

            return View(chatLog);
        }

        // POST: ChatLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatLog = await _context.ChatLogs.FindAsync(id);
            if (chatLog != null)
            {
                _context.ChatLogs.Remove(chatLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatLogExists(int id)
        {
            return _context.ChatLogs.Any(e => e.Id == id);
        }
    }
}
