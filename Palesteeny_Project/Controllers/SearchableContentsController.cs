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
    public class SearchableContentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchableContentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة جملة بحث جديدة", "Create", "SearchableContents"),
        ("التعديل على جمل البحث ", "Index", "SearchableContents"),
        ("حذف جملة", "Index", "SearchableContents"),
        ("عرض البيانات", "Index", "SearchableContents")
    };

            return View(adminActions);
        }
        // GET: SearchableContents
        public async Task<IActionResult> Index()
        {
            return View(await _context.SearchableContents.ToListAsync());
        }

        // GET: SearchableContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchableContent = await _context.SearchableContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (searchableContent == null)
            {
                return NotFound();
            }

            return View(searchableContent);
        }

        // GET: SearchableContents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SearchableContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Url,Type,Description")] SearchableContent searchableContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(searchableContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(searchableContent);
        }

        // GET: SearchableContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchableContent = await _context.SearchableContents.FindAsync(id);
            if (searchableContent == null)
            {
                return NotFound();
            }
            return View(searchableContent);
        }

        // POST: SearchableContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Url,Type,Description")] SearchableContent searchableContent)
        {
            if (id != searchableContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(searchableContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SearchableContentExists(searchableContent.Id))
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
            return View(searchableContent);
        }

        // GET: SearchableContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var searchableContent = await _context.SearchableContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (searchableContent == null)
            {
                return NotFound();
            }

            return View(searchableContent);
        }

        // POST: SearchableContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var searchableContent = await _context.SearchableContents.FindAsync(id);
            if (searchableContent != null)
            {
                _context.SearchableContents.Remove(searchableContent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SearchableContentExists(int id)
        {
            return _context.SearchableContents.Any(e => e.Id == id);
        }
    }
}
