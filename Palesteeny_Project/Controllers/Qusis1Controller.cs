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
    public class Qusis1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Qusis1Controller(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {
        
        ("إضافة قصة جديدة", "Create", "Qusis1"),
        ("التعديل على بيانات القصة", "Index", "Qusis1"),
        ("حذف قصة", "Index", "Qusis1"),
        ("عرض تفاصيل القصص الحالية", "Index", "Qusis1")
    };

            return View(adminActions);
        }
        // GET: Qusis1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Qusis.ToListAsync());
        }

        // GET: Qusis1/Details/5
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

        // GET: Qusis1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Qusis1/Create
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

        // GET: Qusis1/Edit/5
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

        // POST: Qusis1/Edit/5
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

        // GET: Qusis1/Delete/5
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

        // POST: Qusis1/Delete/5
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
