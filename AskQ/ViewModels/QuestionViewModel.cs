using System;
using System.Collections.Generic;

namespace AskQ.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string? AskedFromUsername { get; set; }
        public string QuestionText { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public IEnumerable<string>? Replies { get; set; }
    }
}
