using System;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Data;
using AskQ.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskQ.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(string username, string questionText, string receivingId)
        {
            if (string.IsNullOrWhiteSpace(questionText))
            {
                return BadRequest();
            }

            IdentityUser? askedTo = await _dbContext.Users.FindAsync(receivingId);
            if (askedTo is null)
            {
                return NotFound();
            }

            IdentityUser? askedFrom = null;
            if (User.Identity.IsAuthenticated)
            {
                askedFrom = await _dbContext.Users.FirstAsync(u => u.UserName == User.Identity.Name);
            }

            var newQuestion = new Question
            {
                AskedTo = askedTo,
                AskedFrom = askedFrom,
                QuestionText = questionText,
                DateTime = DateTime.Now,
                Replies = Enumerable.Empty<Reply>()
            };
            await _dbContext.Questions.AddAsync(newQuestion);
            await _dbContext.SaveChangesAsync();
            TempData["Message"] = "Your question has been sent. Questions appear only after they are answered.";
            return RedirectToAction("UserProfile", "User", new { username = askedTo.UserName });
        }

        [HttpGet]
        public IActionResult Received()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            return View(_dbContext.Questions.Include(q => q.AskedTo).Where(q => q.AskedTo.UserName == User.Identity.Name && !q.Replies.Any()).AsEnumerable());
        }
    }
}
