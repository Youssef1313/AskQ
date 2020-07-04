using System.Linq;
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

       /* [HttpPost]
        public IActionResult PostQuestion(string username, string question)
        {

        }*/
    }
}
