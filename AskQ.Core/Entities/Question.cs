using System;
using System.Collections.Generic;

namespace AskQ.Core.Entities
{
    public class Question
    {
        // The nullable annotation should help EF Core to determine whether the property is required or not.
        public int Id { get; set; }
        public string QuestionText { get; set; } = null!;
        public string AskedToGuid { get; set; } = null!;
        public string? AskedFromGuid { get; set; }
        public IEnumerable<Reply> Replies { get; set; } = null!;
        public DateTime DateTime { get; set; }
    }
}
