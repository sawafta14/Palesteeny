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
    public class UserLessonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserLessonsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

      
      
        ("حذف تقدم طالب", "Delete", "UserLessons"),
        ("عرض بيانات تقدم الطلاب", "Details", "UserLessons")
    };

            return View(adminActions);
        }

        // GET: UserLessons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserLessons.Include(u => u.Lesson).Include(u => u.UserPal);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserLessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLesson = await _context.UserLessons
                .Include(u => u.Lesson)
                .Include(u => u.UserPal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLesson == null)
            {
                return NotFound();
            }

            return View(userLesson);
        }

        // GET: UserLessons/Create
        public IActionResult Create()
        {
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId");
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email");
            return View();
        }

        // POST: UserLessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserPalId,LessonId,IsCompleted,StartTime,EndTime,BookmarkPage")] UserLesson userLesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userLesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId", userLesson.LessonId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", userLesson.UserPalId);
            return View(userLesson);
        }

        // GET: UserLessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLesson = await _context.UserLessons.FindAsync(id);
            if (userLesson == null)
            {
                return NotFound();
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId", userLesson.LessonId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", userLesson.UserPalId);
            return View(userLesson);
        }

        // POST: UserLessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserPalId,LessonId,IsCompleted,StartTime,EndTime,BookmarkPage")] UserLesson userLesson)
        {
            if (id != userLesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userLesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLessonExists(userLesson.Id))
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
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId", userLesson.LessonId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", userLesson.UserPalId);
            return View(userLesson);
        }

        // GET: UserLessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLesson = await _context.UserLessons
                .Include(u => u.Lesson)
                .Include(u => u.UserPal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLesson == null)
            {
                return NotFound();
            }

            return View(userLesson);
        }

        // POST: UserLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userLesson = await _context.UserLessons.FindAsync(id);
            if (userLesson != null)
            {
                _context.UserLessons.Remove(userLesson);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserLessonExists(int id)
        {
            return _context.UserLessons.Any(e => e.Id == id);
        }
    }
}
