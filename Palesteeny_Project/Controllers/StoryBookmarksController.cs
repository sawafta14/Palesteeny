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
    public class StoryBookmarksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoryBookmarksController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        
       
        
        ("عرض الاشارات المرجعية ", "Details", "StoryBookmarks")
    };

            return View(adminActions);
        }
        // GET: StoryBookmarks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StoryBookmarks.Include(s => s.Qusi).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StoryBookmarks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyBookmark = await _context.StoryBookmarks
                .Include(s => s.Qusi)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storyBookmark == null)
            {
                return NotFound();
            }

            return View(storyBookmark);
        }

        // GET: StoryBookmarks/Create
        public IActionResult Create()
        {
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title");
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email");
            return View();
        }

        // POST: StoryBookmarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserPalId,QusiId,LastImageIndex")] StoryBookmark storyBookmark)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storyBookmark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", storyBookmark.QusiId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", storyBookmark.UserPalId);
            return View(storyBookmark);
        }

        // GET: StoryBookmarks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyBookmark = await _context.StoryBookmarks.FindAsync(id);
            if (storyBookmark == null)
            {
                return NotFound();
            }
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", storyBookmark.QusiId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", storyBookmark.UserPalId);
            return View(storyBookmark);
        }

        // POST: StoryBookmarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserPalId,QusiId,LastImageIndex")] StoryBookmark storyBookmark)
        {
            if (id != storyBookmark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storyBookmark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryBookmarkExists(storyBookmark.Id))
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
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", storyBookmark.QusiId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", storyBookmark.UserPalId);
            return View(storyBookmark);
        }

        // GET: StoryBookmarks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storyBookmark = await _context.StoryBookmarks
                .Include(s => s.Qusi)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storyBookmark == null)
            {
                return NotFound();
            }

            return View(storyBookmark);
        }

        // POST: StoryBookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storyBookmark = await _context.StoryBookmarks.FindAsync(id);
            if (storyBookmark != null)
            {
                _context.StoryBookmarks.Remove(storyBookmark);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoryBookmarkExists(int id)
        {
            return _context.StoryBookmarks.Any(e => e.Id == id);
        }
    }
}
