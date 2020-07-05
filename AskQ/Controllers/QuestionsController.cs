using System;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Core.Entities;
using AskQ.Data;
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
        public async Task<IActionResult> PostAsync(string questionText, string receivingId)
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
            return View(_dbContext.Questions.Include(q => q.AskedTo)
                .Where(q => q.AskedTo.UserName == User.Identity.Name && !q.Replies.Any())
                .OrderByDescending(q => q.DateTime)
                .AsEnumerable());
        }

        [HttpGet]
        public async Task<IActionResult> AnswerAsync(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Forbid();
            }
            Question? question = await _dbContext.Questions.Include(q => q.AskedTo).Include(q => q.Replies).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            if (question.AskedTo.UserName != User.Identity.Name)
            {
                return Forbid();
            }
            if (question.Replies.Any())
            {
                return BadRequest();
            }
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> AnswerAsync(int id, string answerText)
        {
            if (string.IsNullOrWhiteSpace(answerText))
            {
                return BadRequest();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return Forbid();
            }
            Question? question = await _dbContext.Questions.Include(q => q.AskedTo).Include(q => q.Replies).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            if (question.AskedTo.UserName != User.Identity.Name)
            {
                return Forbid();
            }
            if (question.Replies.Any())
            {
                return BadRequest();
            }
            IdentityUser user = await _dbContext.Users.FirstAsync(u => u.UserName == User.Identity.Name);
            await _dbContext.Replies.AddAsync(new Reply
            {
                User = user,
                Question = question,
                ReplyText = answerText,
                DateTime = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("UserProfile", "User", new { username = user.UserName });
        }
    }
}
