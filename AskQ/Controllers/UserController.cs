using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AskQ.Data;
using AskQ.Models;
using AskQ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskQ.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET /username
        [HttpGet("{username}")]
        public IActionResult UserProfile(string username, int page = 1)
        {
            IdentityUser user = _dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user is null)
            {
                return NotFound();
            }
            var viewModel = new UserProfileViewModel
            {
                PageNumber = page,
                User = user,
                // TODO: Skip depending on page.
                //Questions = _dbContext.Questions.Include(q => q.Replies).Include(q => q.AskedTo).Where(q => q.AskedTo.UserName == username).Take(10).AsEnumerable()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostQuestionAsync(string username, string questionText, string receivingId)
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
            return RedirectToAction(nameof(UserProfile), new { username = askedTo.UserName });
        }
    }
}
