using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Core.Entities;
using AskQ.Infrastructure.Data;
using AskQ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskQ.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // GET /username
        [HttpGet("{username}")]
        public async Task<IActionResult> UserProfileAsync(string username, int page = 1)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                return NotFound();
            }


            // We'll keep it for now like this, but there are issues generally.
            List<Question> questions = await _dbContext.Questions
                                            .Include(q => q.Replies)
                                            .Where(q => q.AskedToGuid == user.Id && q.Replies.Any()) // This query will be processed in memory. All records will be retrieved from DB.
                                            .Skip(10 * (page - 1))
                                            .Take(10)
                                            .OrderByDescending(q => q.DateTime)
                                            .ToListAsync();

            var questionsViewModel = new List<QuestionViewModel>();
            foreach (Question question in questions)
            {
                questionsViewModel.Add(new QuestionViewModel
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    AskedFromUsername = (await _userManager.FindByIdAsync(question.AskedFromGuid))?.UserName, // Since we need username all the time, we can keep guid and username in Question.
                    DateTime = question.DateTime,
                    Replies = question.Replies.Select(r => r.ReplyText)
                });
            }

            var viewModel = new UserProfileViewModel
            {
                PageNumber = page,
                Username = username,
                UserId = user.Id,
                Questions = questionsViewModel.AsEnumerable()
            };
            return View(viewModel);
        }
    }
}
