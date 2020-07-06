using System.Collections;
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

            IEnumerable<QuestionViewModel> questions = _dbContext.Questions.Include(q => q.Replies)
                    .Where(q => q.AskedToGuid == user.Id && q.Replies.Any())
                    .Skip(10 * (page - 1))
                    .Take(10)
                    .OrderByDescending(q => q.DateTime)
                    .Select(q => new QuestionViewModel
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        AskedFromUsername = (_userManager.FindByIdAsync(q.AskedFromGuid)).Result.UserName,
                        DateTime = q.DateTime,
                        Replies = q.Replies.Select(r => r.ReplyText)
                    }).AsEnumerable(); // Looks too long.
                                       // There could be a better approach to populate IEnumerable<QuestionViewModel> from the IEnumerable<Question>;

            var viewModel = new UserProfileViewModel
            {
                PageNumber = page,
                Username = username,
                UserId = user.Id,
                Questions = questions
            };
            return View(viewModel);
        }
    }
}
