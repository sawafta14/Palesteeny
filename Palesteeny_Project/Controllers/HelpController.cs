using Microsoft.AspNetCore.Mvc;
using Palesteeny_Project.Models;
using Palesteeny_Project.Services;
using Palesteeny_Project.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Palesteeny_Project.Controllers
{
    public class HelpController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public HelpController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

     
        public IActionResult Help()
        {
            var questions = _context.HelpQuestions.ToList();
            return View(questions);
        }

       
        [HttpPost]
        public async Task<IActionResult> SendFeedback([FromBody] FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            string body = $@"
                <strong>الاسم:</strong> {model.Name}<br/>
                <strong>البريد الإلكتروني:</strong> {model.Email}<br/>
                <strong>نوع الملاحظة:</strong> {model.Type}<br/><br/>
                <strong>الرسالة:</strong><br/>{model.Message?.Replace("\n", "<br/>")}
            ";

            await _emailSender.SendEmailAsync(
                toEmail: "support@palesteeny.ps",
                subject: "💌 ملاحظة جديدة من مركز المساعدة",
                message: body
            );

            return Ok();
        }
    }
}
