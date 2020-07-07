using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Core.Entities;
using AskQ.Infrastructure.Data;
using AskQ.Infrastructure.Identity;
using AskQ.Interfaces;
using AskQ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskQ.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuestionService _questionService;

        public QuestionsController(AppDbContext dbContext,
                                   UserManager<ApplicationUser> userManager,
                                   IQuestionService questionService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _questionService = questionService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(string questionText, string receivingId)
        {
            if (string.IsNullOrWhiteSpace(questionText))
            {
                return BadRequest();
            }

            ApplicationUser? askedToUser = await _userManager.FindByIdAsync(receivingId);
            if (askedToUser is null)
            {
                return NotFound();
            }

            ApplicationUser askedFromUser = await _userManager.GetUserAsync(User);

            await _questionService.CreateQuestionAsync(questionText, askedToUser, askedFromUser);

            TempData["Message"] = "Your question has been sent. Questions appear only after they are answered.";
            return RedirectToAction("UserProfile", "User", new { username = askedToUser.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> ReceivedAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            string authenticatedUserId = (await _userManager.GetUserAsync(User)).Id;

            List<QuestionViewModel> questions = await _questionService.GetQuestionsForUserAsync(authenticatedUserId);

            return View(questions.AsEnumerable());
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
            if (question.AskedToUserId != (await _userManager.GetUserAsync(User)).Id)
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
                QuestionText = question.Text,
                AskedFromUsername = (await _userManager.FindByIdAsync(question.AskedFromUserId))?.UserName, // Since we need username all the time, we can keep guid and username in Question.
                DateTime = question.Date,
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

            if (question.AskedToUserId != (await _userManager.GetUserAsync(User)).Id)
            {
                return Forbid();
            }
            if (question.Replies.Any())
            {
                return BadRequest();
            }

            /*await _dbContext.Replies.AddAsync(new Reply
            {
                UserId = question.AskedToGuid,
                Question = question,
                Text = answerText,
                Date = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();*/
            // TODO: ReplyService.

            return RedirectToAction("UserProfile", "User", new { username = User.Identity.Name });
        }
    }
}
