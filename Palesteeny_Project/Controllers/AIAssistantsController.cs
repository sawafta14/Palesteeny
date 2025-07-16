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
    public class AIAssistantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AIAssistantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AIAssistants
        public async Task<IActionResult> Index()
        {
            return View(await _context.AIAssistant.ToListAsync());
        }

        // GET: AIAssistants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aIAssistant = await _context.AIAssistant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aIAssistant == null)
            {
                return NotFound();
            }

            return View(aIAssistant);
        }

        // GET: AIAssistants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AIAssistants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Color,ImageUrl,Gender,Name")] AIAssistant aIAssistant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aIAssistant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aIAssistant);
        }

        // GET: AIAssistants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aIAssistant = await _context.AIAssistant.FindAsync(id);
            if (aIAssistant == null)
            {
                return NotFound();
            }
            return View(aIAssistant);
        }

        // POST: AIAssistants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Color,ImageUrl,Gender,Name")] AIAssistant aIAssistant)
        {
            if (id != aIAssistant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aIAssistant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AIAssistantExists(aIAssistant.Id))
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
            return View(aIAssistant);
        }

        // GET: AIAssistants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aIAssistant = await _context.AIAssistant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aIAssistant == null)
            {
                return NotFound();
            }

            return View(aIAssistant);
        }

        // POST: AIAssistants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aIAssistant = await _context.AIAssistant.FindAsync(id);
            if (aIAssistant != null)
            {
                _context.AIAssistant.Remove(aIAssistant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AIAssistantExists(int id)
        {
            return _context.AIAssistant.Any(e => e.Id == id);
        }
    }
}
