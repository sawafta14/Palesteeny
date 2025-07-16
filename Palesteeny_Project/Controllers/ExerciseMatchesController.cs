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
    public class ExerciseMatchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseMatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExerciseMatches
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExerciseMatch.Include(e => e.ExerciseQuestion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExerciseMatches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseMatch = await _context.ExerciseMatch
                .Include(e => e.ExerciseQuestion)
                .FirstOrDefaultAsync(m => m.ExerciseMatchId == id);
            if (exerciseMatch == null)
            {
                return NotFound();
            }

            return View(exerciseMatch);
        }

        // GET: ExerciseMatches/Create
        public IActionResult Create()
        {
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId");
            return View();
        }

        // POST: ExerciseMatches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExerciseMatchId,OptionText,MatchLabel,MatchImageUrl,OptionImageUrl,ExerciseQuestionId")] ExerciseMatch exerciseMatch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exerciseMatch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId", exerciseMatch.ExerciseQuestionId);
            return View(exerciseMatch);
        }

        // GET: ExerciseMatches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseMatch = await _context.ExerciseMatch.FindAsync(id);
            if (exerciseMatch == null)
            {
                return NotFound();
            }
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId", exerciseMatch.ExerciseQuestionId);
            return View(exerciseMatch);
        }

        // POST: ExerciseMatches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseMatchId,OptionText,MatchLabel,MatchImageUrl,OptionImageUrl,ExerciseQuestionId")] ExerciseMatch exerciseMatch)
        {
            if (id != exerciseMatch.ExerciseMatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exerciseMatch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseMatchExists(exerciseMatch.ExerciseMatchId))
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
            ViewData["ExerciseQuestionId"] = new SelectList(_context.ExerciseQuestions, "ExerciseQuestionId", "ExerciseQuestionId", exerciseMatch.ExerciseQuestionId);
            return View(exerciseMatch);
        }

        // GET: ExerciseMatches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseMatch = await _context.ExerciseMatch
                .Include(e => e.ExerciseQuestion)
                .FirstOrDefaultAsync(m => m.ExerciseMatchId == id);
            if (exerciseMatch == null)
            {
                return NotFound();
            }

            return View(exerciseMatch);
        }

        // POST: ExerciseMatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exerciseMatch = await _context.ExerciseMatch.FindAsync(id);
            if (exerciseMatch != null)
            {
                _context.ExerciseMatch.Remove(exerciseMatch);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseMatchExists(int id)
        {
            return _context.ExerciseMatch.Any(e => e.ExerciseMatchId == id);
        }
    }
}
