using System.Collections.Generic;

namespace AskQ.ViewModels
{
    public class UserProfileViewModel
    {
        public int PageNumber { get; set; }
        public string? Username { get; set; }
        public string? UserId { get; set; }
        public IEnumerable<QuestionViewModel>? Questions { get; set; }
    }
}
