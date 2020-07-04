using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AskQ.Models
{
    public class Question
    {
        // The nullable annotation should help EF Core to determine whether the property is required or not.
        public int Id { get; set; }
        public string QuestionText { get; set; } = null!;
        public IdentityUser AskedTo { get; set; } = null!;
        public IdentityUser? AskedFrom { get; set; }
        public IEnumerable<Reply> Replies { get; set; } = null!;
        public DateTime DateTime { get; set; }
    }
}
