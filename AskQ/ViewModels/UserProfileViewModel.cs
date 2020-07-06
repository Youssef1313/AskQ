using System.Collections.Generic;

namespace AskQ.ViewModels
{
    public class UserProfileViewModel
    {
        public int PageNumber { get; set; }
        public string Username { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public IEnumerable<QuestionViewModel>? Questions { get; set; }
    }
}
