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
        public IActionResult Manage()
        {
            var adminActions = new List<(string Name, string Action, string Controller)>
    {

        ("إضافة طالب جديد", "Create", "UserPals"),
        ("التعديل على بيانات الطلاب ", "Index", "UserPals"),
        ("حذف طالب", "Index", "UserPals"),
        ("عرض بيانات الطلاب", "Index", "UserPals")
    };

            return View(adminActions);
        }
     
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UsersPal.Include(u => u.Semester);
            return View(await applicationDbContext.ToListAsync());
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPal = await _context.UsersPal
                .Include(u => u.Semester)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPal == null)
            {
                return NotFound();
            }

            return View(userPal);
        }

        
        public IActionResult Create()
        {
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Gender,Age,ImagePath,SemesterId,Id,FirstName,LastName,Email,PasswordHash,EmailConfirmed,ConfirmationToken,Role")] UserPal userPal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId", userPal.SemesterId);
            return View(userPal);
        }

     
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
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId", userPal.SemesterId);
            return View(userPal);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Gender,Age,ImagePath,SemesterId,Id,FirstName,LastName,Email,PasswordHash,EmailConfirmed,ConfirmationToken,Role")] UserPal userPal)
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
            ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId", userPal.SemesterId);
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
                .Include(u => u.Semester)
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
