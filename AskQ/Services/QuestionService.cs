using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskQ.Core.Entities;
using AskQ.Infrastructure.Data;
using AskQ.Interfaces;
using AskQ.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AskQ.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _dbContext;

        public QuestionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<QuestionViewModel>> GetQuestionsForUserAsync(string userId, int page, int pagesize)
        {
            // Separate pagination model could be defined. All appropriate validations should be done to convert page and pagesize to "take" and "skip".
            // Example, what is page argument is negative, what if zero. What if I request pagesize of 1000?

            List<Question> questions = await _dbContext.Questions
                                            .Include(q => q.Replies)
                                            .Where(q => q.AskedToUserId == userId && q.HasReplies)
                                            .Skip(pagesize * (page - 1))
                                            .Take(pagesize)
                                            .OrderByDescending(q => q.Date)
                                            .ToListAsync();

            var questionsViewModel = new List<QuestionViewModel>();
            foreach (Question question in questions)
            {
                questionsViewModel.Add(new QuestionViewModel
                {
                    Id = question.Id,
                    QuestionText = question.Text,
                    AskedFromUsername = question.AskedFromUsername,
                    DateTime = question.Date,
                    Replies = question.Replies.Select(r => r.Text)
                });
            }

            return questionsViewModel;
        }
    }
}
