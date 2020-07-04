using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> UserProfileAsync(string username, int page = 1)
        {
            IdentityUser user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user is null)
            {
                return NotFound();
            }
            var viewModel = new UserProfileViewModel
            {
                PageNumber = page,
                User = user,
                Questions = _dbContext.Questions.Include(q => q.Replies).Include(q => q.AskedTo).Where(q => q.AskedTo.UserName == username && q.Replies.Any()).Skip(10 * (page - 1)).Take(10).AsEnumerable()
            };
            return View(viewModel);
        }
    }
}
