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
    public class FavoriteStoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoriteStoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        
        ("عرض المكتبة الخاصة بالطلاب", "Index", "FavoriteStories")
    };

            return View(adminActions);
        }

        // GET: FavoriteStories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FavoriteStories.Include(f => f.Qusi).Include(f => f.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FavoriteStories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteStory = await _context.FavoriteStories
                .Include(f => f.Qusi)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteStory == null)
            {
                return NotFound();
            }

            return View(favoriteStory);
        }

        // GET: FavoriteStories/Create
        public IActionResult Create()
        {
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title");
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email");
            return View();
        }

        // POST: FavoriteStories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserPalId,QusiId")] FavoriteStory favoriteStory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favoriteStory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", favoriteStory.QusiId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", favoriteStory.UserPalId);
            return View(favoriteStory);
        }

        // GET: FavoriteStories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteStory = await _context.FavoriteStories.FindAsync(id);
            if (favoriteStory == null)
            {
                return NotFound();
            }
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", favoriteStory.QusiId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", favoriteStory.UserPalId);
            return View(favoriteStory);
        }

        // POST: FavoriteStories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserPalId,QusiId")] FavoriteStory favoriteStory)
        {
            if (id != favoriteStory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favoriteStory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteStoryExists(favoriteStory.Id))
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
            ViewData["QusiId"] = new SelectList(_context.Qusis, "Id", "Title", favoriteStory.QusiId);
            ViewData["UserPalId"] = new SelectList(_context.UsersPal, "Id", "Email", favoriteStory.UserPalId);
            return View(favoriteStory);
        }

        // GET: FavoriteStories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favoriteStory = await _context.FavoriteStories
                .Include(f => f.Qusi)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favoriteStory == null)
            {
                return NotFound();
            }

            return View(favoriteStory);
        }

        // POST: FavoriteStories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favoriteStory = await _context.FavoriteStories.FindAsync(id);
            if (favoriteStory != null)
            {
                _context.FavoriteStories.Remove(favoriteStory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteStoryExists(int id)
        {
            return _context.FavoriteStories.Any(e => e.Id == id);
        }
    }
}
