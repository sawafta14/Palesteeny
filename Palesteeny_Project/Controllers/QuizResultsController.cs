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
    public class QuizResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizResultsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

       
        
        
        ("عرض نتائج الطلاب", "Details", "QuizResults")
    };

            return View(adminActions);
        }
        // GET: QuizResults
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuizResults.Include(q => q.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuizResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizResult = await _context.QuizResults
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizResult == null)
            {
                return NotFound();
            }

            return View(quizResult);
        }

        // GET: QuizResults/Create
        public IActionResult Create()
        {
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email");
            return View();
        }

        // POST: QuizResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserPalId,Category,TotalScore,TakenAt")] QuizResult quizResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quizResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", quizResult.UserPalId);
            return View(quizResult);
        }

        // GET: QuizResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizResult = await _context.QuizResults.FindAsync(id);
            if (quizResult == null)
            {
                return NotFound();
            }
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", quizResult.UserPalId);
            return View(quizResult);
        }

        // POST: QuizResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserPalId,Category,TotalScore,TakenAt")] QuizResult quizResult)
        {
            if (id != quizResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizResultExists(quizResult.Id))
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
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", quizResult.UserPalId);
            return View(quizResult);
        }

        // GET: QuizResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizResult = await _context.QuizResults
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizResult == null)
            {
                return NotFound();
            }

            return View(quizResult);
        }

        // POST: QuizResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizResult = await _context.QuizResults.FindAsync(id);
            if (quizResult != null)
            {
                _context.QuizResults.Remove(quizResult);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizResultExists(int id)
        {
            return _context.QuizResults.Any(e => e.Id == id);
        }
    }
}
