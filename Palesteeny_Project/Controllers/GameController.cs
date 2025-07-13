
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;

namespace Palesteeny_Project.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // عرض كل الألعاب
        public async Task<IActionResult> Game()
        {
            var games = await _context.Games.ToListAsync();
            return View(games);
        }

        // عرض لعبة واحدة بناءً على ID
        public async Task<IActionResult> OneGame(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return NotFound();

            return View(game);
        }
    }
}
