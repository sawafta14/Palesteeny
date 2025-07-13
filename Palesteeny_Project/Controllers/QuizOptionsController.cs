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
    public class QuizOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة جواب جديد", "Create", "QuizOptions"),
        ("التعديل الاجوبة ", "Index", "QuizOptions"),
        ("حذف جواب", "Index", "QuizOptions"),
        ("عرض الاجابات", "Index", "QuizOptions")
    };

            return View(adminActions);
        }
        // GET: QuizOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuizOptions.Include(q => q.QuizQuestion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuizOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizOption = await _context.QuizOptions
                .Include(q => q.QuizQuestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizOption == null)
            {
                return NotFound();
            }

            return View(quizOption);
        }

        // GET: QuizOptions/Create
        public IActionResult Create()
        {
            ViewData["QuizQuestionId"] = new SelectList(_context.QuizQuestions, "Id", "Id");
            return View();
        }

        // POST: QuizOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OptionText,IsCorrect,QuizQuestionId")] QuizOption quizOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quizOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuizQuestionId"] = new SelectList(_context.QuizQuestions, "Id", "Id", quizOption.QuizQuestionId);
            return View(quizOption);
        }

        // GET: QuizOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizOption = await _context.QuizOptions.FindAsync(id);
            if (quizOption == null)
            {
                return NotFound();
            }
            ViewData["QuizQuestionId"] = new SelectList(_context.QuizQuestions, "Id", "Id", quizOption.QuizQuestionId);
            return View(quizOption);
        }

        // POST: QuizOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OptionText,IsCorrect,QuizQuestionId")] QuizOption quizOption)
        {
            if (id != quizOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizOptionExists(quizOption.Id))
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
            ViewData["QuizQuestionId"] = new SelectList(_context.QuizQuestions, "Id", "Id", quizOption.QuizQuestionId);
            return View(quizOption);
        }

        // GET: QuizOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizOption = await _context.QuizOptions
                .Include(q => q.QuizQuestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizOption == null)
            {
                return NotFound();
            }

            return View(quizOption);
        }

        // POST: QuizOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizOption = await _context.QuizOptions.FindAsync(id);
            if (quizOption != null)
            {
                _context.QuizOptions.Remove(quizOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizOptionExists(int id)
        {
            return _context.QuizOptions.Any(e => e.Id == id);
        }
    }
}
