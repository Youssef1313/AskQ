using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Core.Entities;
using AskQ.Data;
using AskQ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskQ.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public QuestionsController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(string questionText, string receivingId)
        {
            if (string.IsNullOrWhiteSpace(questionText))
            {
                return BadRequest();
            }

            IdentityUser? askedTo = await _userManager.FindByIdAsync(receivingId);
            if (askedTo is null)
            {
                return NotFound();
            }

            string? askedFromGuid = (await _userManager.GetUserAsync(User))?.Id;

            var newQuestion = new Question
            {
                AskedToGuid = receivingId,
                AskedFromGuid = askedFromGuid,
                QuestionText = questionText,
                DateTime = DateTime.UtcNow,
                Replies = Enumerable.Empty<Reply>()
            };
            await _dbContext.Questions.AddAsync(newQuestion);
            await _dbContext.SaveChangesAsync();
            TempData["Message"] = "Your question has been sent. Questions appear only after they are answered.";
            return RedirectToAction("UserProfile", "User", new { username = askedTo.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> ReceivedAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }
            string authenticatedUserGuid = (await _userManager.GetUserAsync(User)).Id;

            List<Question> questions = await _dbContext.Questions
                .Where(q => q.AskedToGuid == authenticatedUserGuid && !q.Replies.Any())
                .OrderByDescending(q => q.DateTime).ToListAsync();

            var questionsViewModel = new List<QuestionViewModel>();
            foreach (Question question in questions)
            {
                questionsViewModel.Add(new QuestionViewModel
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    AskedFromUsername = (await _userManager.FindByIdAsync(question.AskedFromGuid))?.UserName, // Since we need username all the time, we can keep guid and username in Question.
                    DateTime = question.DateTime,
                });
            }

            return View(questionsViewModel.AsEnumerable());
        }

        [HttpGet]
        public async Task<IActionResult> AnswerAsync(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Forbid();
            }
            Question? question = await _dbContext.Questions.Include(q => q.Replies).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            if (question.AskedToGuid != (await _userManager.GetUserAsync(User)).Id)
            {
                return Forbid();
            }
            if (question.Replies.Any())
            {
                return BadRequest();
            }
            return View(new QuestionViewModel
            {
                Id = question.Id,
                QuestionText = question.QuestionText,
                AskedFromUsername = (await _userManager.FindByIdAsync(question.AskedFromGuid))?.UserName, // Since we need username all the time, we can keep guid and username in Question.
                DateTime = question.DateTime,
            });
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
            Question? question = await _dbContext.Questions.Include(q => q.Replies).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            if (question.AskedToGuid != (await _userManager.GetUserAsync(User)).Id)
            {
                return Forbid();
            }
            if (question.Replies.Any())
            {
                return BadRequest();
            }

            await _dbContext.Replies.AddAsync(new Reply
            {
                UserGuid = question.AskedToGuid,
                Question = question,
                ReplyText = answerText,
                DateTime = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("UserProfile", "User", new { username = User.Identity.Name });
        }
    }
}
