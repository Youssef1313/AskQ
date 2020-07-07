using System.Collections.Generic;
using System.Threading.Tasks;
using AskQ.ViewModels;

namespace AskQ.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionViewModel>> GetQuestionsForUserAsync(string userId, int page, int pagesize);
    }
}
