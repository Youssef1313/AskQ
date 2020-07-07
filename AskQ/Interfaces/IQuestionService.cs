using System.Collections.Generic;
using System.Threading.Tasks;
using AskQ.Infrastructure.Identity;
using AskQ.ViewModels;

namespace AskQ.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionViewModel> GetQuestionAsync(int questionId);
        Task<List<QuestionViewModel>> GetQuestionsForUserAsync(string userId);
        Task<List<QuestionViewModel>> GetQuestionsForUserAsync(string userId, int page, int pagesize);
        Task CreateQuestionAsync(string text, ApplicationUser toUser, ApplicationUser? fromUser);
        Task<QuestionViewModel> AnswerQuestionAsync(int questionId, string answerText, ApplicationUser toUser);
    }
}
