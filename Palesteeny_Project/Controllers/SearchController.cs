using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
namespace Palesteeny_Project.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var results = await _context.SearchableContents
                .Where(c =>
                    EF.Functions.Like(c.Title, $"%{term}%") ||
                    EF.Functions.Like(c.Description, $"%{term}%"))
                .ToListAsync();

            var categorized = results
                .GroupBy(r => r.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    Items = g.Select(i => new
                    {
                        i.Title,
                        i.Url,
                        Description = i.Description ?? ""
                    }).ToList()
                }).ToList(); // <- add this here

            return Json(categorized);



        }
    }
}
