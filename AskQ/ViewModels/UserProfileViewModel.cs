using System.Collections.Generic;
using AskQ.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace AskQ.ViewModels
{
    public class UserProfileViewModel
    {
        public int PageNumber { get; set; }
        public string Username { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public IEnumerable<Question>? Questions { get; set; }
    }
}
