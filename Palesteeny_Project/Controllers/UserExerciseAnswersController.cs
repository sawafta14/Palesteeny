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
    public class UserExerciseAnswersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserExerciseAnswersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة جواب جديد", "Create", "UserExerciseAnswers"),
        ("التعديل على الاجوبة ", "Index", "UserExerciseAnswers"),
        ("حذف جواب", "Index", "UserExerciseAnswers"),
        ("عرض االصفوف الحالية", "Index", "UserExerciseAnswers")
    };

            return View(adminActions);
        }

        // GET: UserExerciseAnswers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserExerciseAnswers.Include(u => u.ExerciseOption);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserExerciseAnswers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExerciseAnswer = await _context.UserExerciseAnswers
                .Include(u => u.ExerciseOption)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExerciseAnswer == null)
            {
                return NotFound();
            }

            return View(userExerciseAnswer);
        }

        // GET: UserExerciseAnswers/Create
        public IActionResult Create()
        {
            ViewData["ExerciseOptionId"] = new SelectList(_context.ExerciseOptions, "ExerciseOptionId", "ExerciseOptionId");
            return View();
        }

        // POST: UserExerciseAnswers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ExerciseOptionId,UserAnswer,IsCorrect")] UserExerciseAnswer userExerciseAnswer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userExerciseAnswer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseOptionId"] = new SelectList(_context.ExerciseOptions, "ExerciseOptionId", "ExerciseOptionId", userExerciseAnswer.ExerciseOptionId);
            return View(userExerciseAnswer);
        }

        // GET: UserExerciseAnswers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExerciseAnswer = await _context.UserExerciseAnswers.FindAsync(id);
            if (userExerciseAnswer == null)
            {
                return NotFound();
            }
            ViewData["ExerciseOptionId"] = new SelectList(_context.ExerciseOptions, "ExerciseOptionId", "ExerciseOptionId", userExerciseAnswer.ExerciseOptionId);
            return View(userExerciseAnswer);
        }

        // POST: UserExerciseAnswers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ExerciseOptionId,UserAnswer,IsCorrect")] UserExerciseAnswer userExerciseAnswer)
        {
            if (id != userExerciseAnswer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userExerciseAnswer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExerciseAnswerExists(userExerciseAnswer.Id))
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
            ViewData["ExerciseOptionId"] = new SelectList(_context.ExerciseOptions, "ExerciseOptionId", "ExerciseOptionId", userExerciseAnswer.ExerciseOptionId);
            return View(userExerciseAnswer);
        }

        // GET: UserExerciseAnswers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExerciseAnswer = await _context.UserExerciseAnswers
                .Include(u => u.ExerciseOption)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExerciseAnswer == null)
            {
                return NotFound();
            }

            return View(userExerciseAnswer);
        }

        // POST: UserExerciseAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userExerciseAnswer = await _context.UserExerciseAnswers.FindAsync(id);
            if (userExerciseAnswer != null)
            {
                _context.UserExerciseAnswers.Remove(userExerciseAnswer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExerciseAnswerExists(int id)
        {
            return _context.UserExerciseAnswers.Any(e => e.Id == id);
        }
    }
}
