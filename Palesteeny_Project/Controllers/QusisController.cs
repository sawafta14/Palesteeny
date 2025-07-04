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
    public class QusisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QusisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Qusis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Qusis.ToListAsync());
        }

        // GET: Qusis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qusi = await _context.Qusis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qusi == null)
            {
                return NotFound();
            }

            return View(qusi);
        }

        // GET: Qusis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Qusis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Category,CoverImage,Link")] Qusi qusi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qusi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qusi);
        }

        // GET: Qusis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qusi = await _context.Qusis.FindAsync(id);
            if (qusi == null)
            {
                return NotFound();
            }
            return View(qusi);
        }

        // POST: Qusis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,CoverImage,Link")] Qusi qusi)
        {
            if (id != qusi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qusi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QusiExists(qusi.Id))
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
            return View(qusi);
        }

        // GET: Qusis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qusi = await _context.Qusis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qusi == null)
            {
                return NotFound();
            }

            return View(qusi);
        }

        // POST: Qusis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qusi = await _context.Qusis.FindAsync(id);
            if (qusi != null)
            {
                _context.Qusis.Remove(qusi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QusiExists(int id)
        {
            return _context.Qusis.Any(e => e.Id == id);
        }
    }
}
