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
    public class DrawingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrawingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Drawings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Drawings.Include(d => d.Template).Include(d => d.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Drawings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drawing = await _context.Drawings
                .Include(d => d.Template)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DrawingId == id);
            if (drawing == null)
            {
                return NotFound();
            }

            return View(drawing);
        }

        // GET: Drawings/Create
        public IActionResult Create()
        {
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id");
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email");
            return View();
        }

        // POST: Drawings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DrawingId,UserPalId,ImageUrl,Mode,TemplateId,BrushType,Color")] Drawing drawing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drawing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id", drawing.TemplateId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", drawing.UserPalId);
            return View(drawing);
        }

        // GET: Drawings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drawing = await _context.Drawings.FindAsync(id);
            if (drawing == null)
            {
                return NotFound();
            }
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id", drawing.TemplateId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", drawing.UserPalId);
            return View(drawing);
        }

        // POST: Drawings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DrawingId,UserPalId,ImageUrl,Mode,TemplateId,BrushType,Color")] Drawing drawing)
        {
            if (id != drawing.DrawingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drawing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrawingExists(drawing.DrawingId))
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
            ViewData["TemplateId"] = new SelectList(_context.Templates, "Id", "Id", drawing.TemplateId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", drawing.UserPalId);
            return View(drawing);
        }

        // GET: Drawings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drawing = await _context.Drawings
                .Include(d => d.Template)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DrawingId == id);
            if (drawing == null)
            {
                return NotFound();
            }

            return View(drawing);
        }

        // POST: Drawings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drawing = await _context.Drawings.FindAsync(id);
            if (drawing != null)
            {
                _context.Drawings.Remove(drawing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrawingExists(int id)
        {
            return _context.Drawings.Any(e => e.DrawingId == id);
        }
    }
}
