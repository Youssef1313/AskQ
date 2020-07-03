using System;
using Microsoft.AspNetCore.Identity;

namespace AskQ.Models
{
    public class Reply
    {
        public Question Question { get; set; } = null!;
        public string ReplyText { get; set; } = null!;
        public IdentityUser? User { get; set; } // Nullable because I'm allowing annonymous.
        public DateTime DateTime { get; set; }
    }
}
