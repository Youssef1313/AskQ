using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Infrastructure.Identity;
using AskQ.Interfaces;
using AskQ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AskQ.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuestionService _questionService;

        public UserController(UserManager<ApplicationUser> userManager,
                              IQuestionService questionService)
        {
            _questionService = questionService;
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

            List<QuestionViewModel> questions = await _questionService.GetQuestionsForUserAsync(user.Id, page, 10, hasReplies: true);

            var viewModel = new UserProfileViewModel
            {
                PageNumber = page,
                Username = username,
                UserId = user.Id,
                Questions = questions.AsEnumerable()
            };
            return View(viewModel);
        }
    }
}
