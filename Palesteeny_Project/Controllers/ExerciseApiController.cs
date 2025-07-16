using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Palesteeny_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExerciseApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // جلب التمارين حسب الدرس مع إجابات المستخدم (إن وجدت)
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetLessonExercises(int lessonId)
        {
            // الحصول على معرّف المستخدم الحالي (تأكد أن المستخدم مسجل دخول)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var groups = await _context.QuestionGroups
                .Where(g => g.LessonId == lessonId)
                .Include(g => g.Questions)
                    .ThenInclude(q => q.Options)
                .Include(g => g.Questions)
                    .ThenInclude(q => q.Matches)
                .ToListAsync();

            // جلب إجابات المستخدم المرتبطة بالخيارات المعروضة
            var optionIds = groups.SelectMany(g => g.Questions)
                                  .SelectMany(q => q.Options)
                                  .Select(o => o.ExerciseOptionId)
                                  .ToList();

            var userAnswers = await _context.UserExerciseAnswers
                .Where(a => a.UserId == userId && optionIds.Contains(a.ExerciseOptionId))
                .ToListAsync();

            var result = groups.Select(g => new
            {
                id = g.QuestionGroupId,
                type = g.Type ?? "select_option",
                question = g.Title ?? "",
                questionOverlay = g.QuestionOverlay ?? false,
                image = g.SharedImageUrl ?? "",
                questions = g.Questions.Select(q => new
                {
                    id = q.ExerciseQuestionId,
                    question = q.Question ?? "",
                    options = q.Options.Select(o =>
                    {
                        var ua = userAnswers.FirstOrDefault(x => x.ExerciseOptionId == o.ExerciseOptionId);
                        return new
                        {
                            id = o.ExerciseOptionId,
                            text = o.OptionQuestion ?? "",
                            image = o.OptionImageUrl ?? "",
                            answer = o.Answer,
                            userAnswer = ua?.UserAnswer,
                            isCorrect = ua?.IsCorrect
                        };
                    }).ToList(),
                    matches = q.Matches.Select(m => new
                    {
                        optionText = m.OptionText ?? "",
                        optionImageUrl = m.OptionImageUrl ?? "",
                        matchLabel = m.MatchLabel ?? "",
                        matchImageUrl = m.MatchImageUrl ?? ""
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // حفظ إجابات المستخدم على الخيارات (أو تحديثها)
        [HttpPost("SubmitAnswers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<ExerciseOptionUpdateDto> options)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            foreach (var item in options)
            {
                var correctOption = await _context.ExerciseOptions.FindAsync(item.ExerciseOptionId);
                if (correctOption == null) continue;

                var userAnswer = await _context.UserExerciseAnswers
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.ExerciseOptionId == item.ExerciseOptionId);

                bool? isCorrect = item.UserAnswer == null ? null : item.UserAnswer.Trim() == correctOption.Answer?.Trim();

                if (userAnswer == null)
                {
                    userAnswer = new UserExerciseAnswer
                    {
                        UserId = userId,
                        ExerciseOptionId = item.ExerciseOptionId,
                        UserAnswer = item.UserAnswer,
                        IsCorrect = isCorrect
                    };
                    _context.UserExerciseAnswers.Add(userAnswer);
                }
                else
                {
                    userAnswer.UserAnswer = item.UserAnswer;
                    userAnswer.IsCorrect = isCorrect;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "تم التحقق من إجاباتك." });
        }

    

    }

    public class ExerciseOptionUpdateDto
    {
        public int ExerciseOptionId { get; set; }
        public string? UserAnswer { get; set; }
    }
}
