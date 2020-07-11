using System.Collections.Generic;
using PozitronDev.Validations;

namespace AskQ.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel(int pageNumber, string username,
            string userId, IEnumerable<QuestionViewModel> questions)
        {
            PozValidate.For.NullOrEmpty(username, nameof(username));
            PozValidate.For.NullOrEmpty(userId, nameof(userId));
            PozValidate.For.Null(questions, nameof(questions));
            PageNumber = pageNumber;
            Username = username;
            UserId = userId;
            Questions = questions;
        }

        public int PageNumber { get; }
        public string Username { get; }
        public string UserId { get; }
        public IEnumerable<QuestionViewModel> Questions { get; }
    }
}
