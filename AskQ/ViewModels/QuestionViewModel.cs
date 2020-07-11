using System;
using System.Collections.Generic;
using PozitronDev.Validations;

namespace AskQ.ViewModels
{
    public class QuestionViewModel
    {
        public QuestionViewModel(int id, string? askedFromUsername, string text,
            DateTime date, IEnumerable<string> replies)
        {
            PozValidate.For.NullOrEmpty(text, nameof(text));
            PozValidate.For.Null(replies, nameof(replies));
            Id = id;
            AskedFromUsername = askedFromUsername;
            Text = text;
            Date = date;
            Replies = replies;
        }
        public int Id { get; }
        public string? AskedFromUsername { get; }
        public string Text { get; }
        public DateTime Date { get; }
        public IEnumerable<string> Replies { get; }
    }
}
