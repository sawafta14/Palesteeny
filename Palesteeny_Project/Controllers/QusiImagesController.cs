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
    public class QusiImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QusiImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QusiImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QusiImages.Include(q => q.Qusi);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QusiImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qusiImage = await _context.QusiImages
                .Include(q => q.Qusi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qusiImage == null)
            {
                return NotFound();
            }

            return View(qusiImage);
        }

        // GET: QusiImages/Create
        public IActionResult Create()
        {
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title");
            return View();
        }

        // POST: QusiImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QusiId,ImageUrl,Order")] QusiImage qusiImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qusiImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", qusiImage.QusiId);
            return View(qusiImage);
        }

        // GET: QusiImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qusiImage = await _context.QusiImages.FindAsync(id);
            if (qusiImage == null)
            {
                return NotFound();
            }
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", qusiImage.QusiId);
            return View(qusiImage);
        }

        // POST: QusiImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QusiId,ImageUrl,Order")] QusiImage qusiImage)
        {
            if (id != qusiImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qusiImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QusiImageExists(qusiImage.Id))
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
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", qusiImage.QusiId);
            return View(qusiImage);
        }

        // GET: QusiImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qusiImage = await _context.QusiImages
                .Include(q => q.Qusi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qusiImage == null)
            {
                return NotFound();
            }

            return View(qusiImage);
        }

        // POST: QusiImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qusiImage = await _context.QusiImages.FindAsync(id);
            if (qusiImage != null)
            {
                _context.QusiImages.Remove(qusiImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QusiImageExists(int id)
        {
            return _context.QusiImages.Any(e => e.Id == id);
        }
    }
}
