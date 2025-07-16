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
    public class QuestionGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }
      

        // GET: QuestionGroups
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuestionGroups.Include(q => q.Lesson);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuestionGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionGroup = await _context.QuestionGroups
                .Include(q => q.Lesson)
                .FirstOrDefaultAsync(m => m.QuestionGroupId == id);
            if (questionGroup == null)
            {
                return NotFound();
            }

            return View(questionGroup);
        }

        // GET: QuestionGroups/Create
        public IActionResult Create()
        {
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId");
            return View();
        }

        // POST: QuestionGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionGroupId,Title,SharedImageUrl,QuestionOverlay,Type,LessonId")] QuestionGroup questionGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId", questionGroup.LessonId);
            return View(questionGroup);
        }

        // GET: QuestionGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionGroup = await _context.QuestionGroups.FindAsync(id);
            if (questionGroup == null)
            {
                return NotFound();
            }
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId", questionGroup.LessonId);
            return View(questionGroup);
        }

        // POST: QuestionGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionGroupId,Title,SharedImageUrl,QuestionOverlay,Type,LessonId")] QuestionGroup questionGroup)
        {
            if (id != questionGroup.QuestionGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionGroupExists(questionGroup.QuestionGroupId))
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
            ViewData["LessonId"] = new SelectList(_context.Lessons, "LessonId", "LessonId", questionGroup.LessonId);
            return View(questionGroup);
        }

        // GET: QuestionGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionGroup = await _context.QuestionGroups
                .Include(q => q.Lesson)
                .FirstOrDefaultAsync(m => m.QuestionGroupId == id);
            if (questionGroup == null)
            {
                return NotFound();
            }

            return View(questionGroup);
        }

        // POST: QuestionGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionGroup = await _context.QuestionGroups.FindAsync(id);
            if (questionGroup != null)
            {
                _context.QuestionGroups.Remove(questionGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionGroupExists(int id)
        {
            return _context.QuestionGroups.Any(e => e.QuestionGroupId == id);
        }
    }
}
