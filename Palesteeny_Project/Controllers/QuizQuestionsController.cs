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
    public class QuizQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة اختبار جديد", "Create", "QuizQuestions"),
        ("التعديل على اسئلة الاختبار ", "Index", "QuizQuestions"),
        ("حذف سؤال", "Index", "QuizQuestions"),
        ("عرض الاختبارات والاسئلة", "Index", "QuizQuestions")
    };

            return View(adminActions);
        }
        // GET: QuizQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuizQuestions.ToListAsync());
        }

        // GET: QuizQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            return View(quizQuestion);
        }

        // GET: QuizQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuizQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionText,ImageUrl,Score,Category")] QuizQuestion quizQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quizQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quizQuestion);
        }

        // GET: QuizQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestions.FindAsync(id);
            if (quizQuestion == null)
            {
                return NotFound();
            }
            return View(quizQuestion);
        }

        // POST: QuizQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionText,ImageUrl,Score,Category")] QuizQuestion quizQuestion)
        {
            if (id != quizQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizQuestionExists(quizQuestion.Id))
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
            return View(quizQuestion);
        }

        // GET: QuizQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            return View(quizQuestion);
        }

        // POST: QuizQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizQuestion = await _context.QuizQuestions.FindAsync(id);
            if (quizQuestion != null)
            {
                _context.QuizQuestions.Remove(quizQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizQuestionExists(int id)
        {
            return _context.QuizQuestions.Any(e => e.Id == id);
        }
    }
}
