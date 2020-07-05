using System.Collections.Generic;
using AskQ.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace AskQ.ViewModels
{
    public class UserProfileViewModel
    {
        public int PageNumber { get; set; }
        public IdentityUser User { get; set; } = null!;
        public IEnumerable<Question>? Questions { get; set; }
    }
}
