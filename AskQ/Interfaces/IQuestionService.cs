using System.Collections.Generic;
using System.Threading.Tasks;
using AskQ.Infrastructure.Identity;
using AskQ.ViewModels;

namespace AskQ.Interfaces
{
    public interface IQuestionService
    {
        Task CreateQuestionAsync(string text, ApplicationUser toUser, ApplicationUser? fromUser);
        Task<List<QuestionViewModel>> GetQuestionsForUserAsync(string userId, int page, int pagesize);
    }
}
