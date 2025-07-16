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
    public class ExerciseOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة جواب جديد", "Create", "ExerciseOptions"),
        ("التعديل الاجوبة ", "Index", "ExerciseOptions"),
        ("حذف جواب", "Index", "ExerciseOptions"),
        ("عرض الاجابات", "Index", "ExerciseOptions")
    };

            return View(adminActions);
        }
        // GET: ExerciseOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExerciseOptions.Include(e => e.ExerciseQuestion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExerciseOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseOption = await _context.ExerciseOptions
                .Include(e => e.ExerciseQuestion)
                .FirstOrDefaultAsync(m => m.ExerciseOptionId == id);
            if (exerciseOption == null)
            {
                return NotFound();
            }

            return View(exerciseOption);
        }

        // GET: ExerciseOptions/Create
        public IActionResult Create()
        {
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId");
            return View();
        }

        // POST: ExerciseOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExerciseOptionId,OptionQuestion,OptionImageUrl,Answer,ExerciseQuestionId")] ExerciseOption exerciseOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exerciseOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId", exerciseOption.ExerciseQuestionId);
            return View(exerciseOption);
        }

        // GET: ExerciseOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseOption = await _context.ExerciseOptions.FindAsync(id);
            if (exerciseOption == null)
            {
                return NotFound();
            }
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId", exerciseOption.ExerciseQuestionId);
            return View(exerciseOption);
        }

        // POST: ExerciseOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseOptionId,OptionQuestion,OptionImageUrl,Answer,ExerciseQuestionId")] ExerciseOption exerciseOption)
        {
            if (id != exerciseOption.ExerciseOptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exerciseOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseOptionExists(exerciseOption.ExerciseOptionId))
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
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId", exerciseOption.ExerciseQuestionId);
            return View(exerciseOption);
        }

        // GET: ExerciseOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseOption = await _context.ExerciseOptions
                .Include(e => e.ExerciseQuestion)
                .FirstOrDefaultAsync(m => m.ExerciseOptionId == id);
            if (exerciseOption == null)
            {
                return NotFound();
            }

            return View(exerciseOption);
        }

        // POST: ExerciseOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exerciseOption = await _context.ExerciseOptions.FindAsync(id);
            if (exerciseOption != null)
            {
                _context.ExerciseOptions.Remove(exerciseOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseOptionExists(int id)
        {
            return _context.ExerciseOptions.Any(e => e.ExerciseOptionId == id);
        }
    }
}
