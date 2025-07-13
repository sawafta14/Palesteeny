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
    public class HelpQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HelpQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة سؤال مساعد جديد", "Create", "HelpQuestions"),
        ("التعديل على بيانات الصفحة ", "Index", "HelpQuestions"),
        ("حذف سؤال", "Index", "HelpQuestions"),
        ("عرض بيانات الصفحة", "Index", "HelpQuestions")
    };

            return View(adminActions);
        }
        // GET: HelpQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.HelpQuestions.ToListAsync());
        }

        // GET: HelpQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpQuestion = await _context.HelpQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (helpQuestion == null)
            {
                return NotFound();
            }

            return View(helpQuestion);
        }

        // GET: HelpQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HelpQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] HelpQuestion helpQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(helpQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(helpQuestion);
        }

        // GET: HelpQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpQuestion = await _context.HelpQuestions.FindAsync(id);
            if (helpQuestion == null)
            {
                return NotFound();
            }
            return View(helpQuestion);
        }

        // POST: HelpQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] HelpQuestion helpQuestion)
        {
            if (id != helpQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(helpQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HelpQuestionExists(helpQuestion.Id))
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
            return View(helpQuestion);
        }

        // GET: HelpQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpQuestion = await _context.HelpQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (helpQuestion == null)
            {
                return NotFound();
            }

            return View(helpQuestion);
        }

        // POST: HelpQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var helpQuestion = await _context.HelpQuestions.FindAsync(id);
            if (helpQuestion != null)
            {
                _context.HelpQuestions.Remove(helpQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HelpQuestionExists(int id)
        {
            return _context.HelpQuestions.Any(e => e.Id == id);
        }
    }
}
