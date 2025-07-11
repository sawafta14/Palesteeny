using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using Palesteeny_Project.ViewModels;

public class TopThreeHeroesViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public TopThreeHeroesViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var topThree = await _context.QuizResults
            .Include(r => r.User)
            .GroupBy(r => r.UserPalId)
            .Select(g => new HeroViewModel
            {
                User = g.First().User,
                TotalScore = g.Sum(r => r.TotalScore)
            })
            .OrderByDescending(x => x.TotalScore)
            .Take(3)
            .ToListAsync();

        return View(topThree);
    }

}
