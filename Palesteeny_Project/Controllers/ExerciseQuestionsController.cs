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
    public class ExerciseQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة سؤال جديد", "Create", "ExerciseQuestions"),
        ("التعديل على الاسئلة ", "Index", "ExerciseQuestions"),
        ("حذف سؤال", "Index", "ExerciseQuestions"),
        ("عرض الاسئلة", "Index", "ExerciseQuestions")
    };

            return View(adminActions);
        }

        // GET: ExerciseQuestions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExerciseQuestions.Include(e => e.QuestionGroup);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExerciseQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseQuestion = await _context.ExerciseQuestions
                .Include(e => e.QuestionGroup)
                .FirstOrDefaultAsync(m => m.ExerciseQuestionId == id);
            if (exerciseQuestion == null)
            {
                return NotFound();
            }

            return View(exerciseQuestion);
        }

        // GET: ExerciseQuestions/Create
        public IActionResult Create()
        {
            ViewData["QuestionGroupId"] = new SelectList(_context.QuestionGroups, "QuestionGroupId", "QuestionGroupId");
            return View();
        }

        // POST: ExerciseQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExerciseQuestionId,Question,QuestionGroupId")] ExerciseQuestion exerciseQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exerciseQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionGroupId"] = new SelectList(_context.QuestionGroups, "QuestionGroupId", "QuestionGroupId", exerciseQuestion.QuestionGroupId);
            return View(exerciseQuestion);
        }

        // GET: ExerciseQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseQuestion = await _context.ExerciseQuestions.FindAsync(id);
            if (exerciseQuestion == null)
            {
                return NotFound();
            }
            ViewData["QuestionGroupId"] = new SelectList(_context.QuestionGroups, "QuestionGroupId", "QuestionGroupId", exerciseQuestion.QuestionGroupId);
            return View(exerciseQuestion);
        }

        // POST: ExerciseQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseQuestionId,Question,QuestionGroupId")] ExerciseQuestion exerciseQuestion)
        {
            if (id != exerciseQuestion.ExerciseQuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exerciseQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseQuestionExists(exerciseQuestion.ExerciseQuestionId))
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
            ViewData["QuestionGroupId"] = new SelectList(_context.QuestionGroups, "QuestionGroupId", "QuestionGroupId", exerciseQuestion.QuestionGroupId);
            return View(exerciseQuestion);
        }

        // GET: ExerciseQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseQuestion = await _context.ExerciseQuestions
                .Include(e => e.QuestionGroup)
                .FirstOrDefaultAsync(m => m.ExerciseQuestionId == id);
            if (exerciseQuestion == null)
            {
                return NotFound();
            }

            return View(exerciseQuestion);
        }

        // POST: ExerciseQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exerciseQuestion = await _context.ExerciseQuestions.FindAsync(id);
            if (exerciseQuestion != null)
            {
                _context.ExerciseQuestions.Remove(exerciseQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseQuestionExists(int id)
        {
            return _context.ExerciseQuestions.Any(e => e.ExerciseQuestionId == id);
        }
    }
}
