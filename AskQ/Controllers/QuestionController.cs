﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Infrastructure.Identity;
using AskQ.Interfaces;
using AskQ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AskQ.Controllers
{
    public class QuestionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuestionService _questionService;

        public QuestionController(UserManager<ApplicationUser> userManager, IQuestionService questionService)
        {
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

            List<QuestionViewModel> questions = await _questionService.GetQuestionsForUserAsync(authenticatedUserId, hasReplies: false);

            return View(questions.AsEnumerable());
        }

        [HttpGet]
        public async Task<IActionResult> AnswerAsync(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Forbid();
            }
            string userId = (await _userManager.GetUserAsync(User)).Id;
            QuestionViewModel? question =
                (await _questionService.GetQuestionsForUserAsync(userId, hasReplies: false)).FirstOrDefault(q => q.Id == id); // TODO: Need try/catch in case not found??
            // TODO: Check if it is okay if the service return the Model, and have extension methods to convert to ViewModel. It'll be easier.

            if (question == null)
            {
                return NotFound();
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

            string userId = (await _userManager.GetUserAsync(User)).Id;
            QuestionViewModel? question =
                (await _questionService.GetQuestionsForUserAsync(userId, hasReplies: false)).FirstOrDefault(q => q.Id == id); // TODO: get by id directly??

            if (question == null)
            {
                return NotFound();
            }

            ApplicationUser replyWriter = await _userManager.GetUserAsync(User); // TODO: GetUserAsync already called before. Re-use the value
            await _questionService.AnswerQuestionAsync(id, answerText, replyWriter);
            return RedirectToAction("UserProfile", "User", new { username = User.Identity.Name });
        }

        [HttpGet]
        public async Task<IActionResult> ReplyAsync(int id)
        {
            QuestionViewModel question = await _questionService.GetQuestionAsync(id); // throws if not found.
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> ReplyAsync(int id, string replyText)
        {
            if (string.IsNullOrWhiteSpace(replyText))
            {
                return BadRequest();
            }
            QuestionViewModel question = await _questionService.GetQuestionAsync(id); // throws exception if not found.
            if (!question.Replies.Any())
            {
                return BadRequest(); // A question without any replies should first have an answer, then have replies.
            }

            ApplicationUser? replyWriter = await _userManager.GetUserAsync(User);
            await _questionService.AnswerQuestionAsync(id, replyText, replyWriter);
            return RedirectToAction("Details", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            QuestionViewModel question = await _questionService.GetQuestionAsync(id); // throws exception if not found.
            return View(question);
        }
    }
}
