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
    public class UserPalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserPalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserPals
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsersPal.ToListAsync());
        }

        // GET: UserPals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPal = await _context.UsersPal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPal == null)
            {
                return NotFound();
            }

            return View(userPal);
        }

        // GET: UserPals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserPals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PasswordHash,Gender,Grade,Age,ImagePath")] UserPal userPal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userPal);
        }

        // GET: UserPals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPal = await _context.UsersPal.FindAsync(id);
            if (userPal == null)
            {
                return NotFound();
            }
            return View(userPal);
        }

        // POST: UserPals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PasswordHash,Gender,Grade,Age,ImagePath")] UserPal userPal)
        {
            if (id != userPal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPalExists(userPal.Id))
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
            return View(userPal);
        }

        // GET: UserPals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPal = await _context.UsersPal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPal == null)
            {
                return NotFound();
            }

            return View(userPal);
        }

        // POST: UserPals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userPal = await _context.UsersPal.FindAsync(id);
            if (userPal != null)
            {
                _context.UsersPal.Remove(userPal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPalExists(int id)
        {
            return _context.UsersPal.Any(e => e.Id == id);
        }
    }
}
