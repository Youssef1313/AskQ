using System;
using PozitronDev.Validations;

namespace AskQ.Core.Entities
{
    public class Reply
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public string? UserId { get; private set; }
        public string? UserName { get; private set; }
        public DateTime Date { get; private set; }

        public int QuestionId { get; private set; } // TODO: This is unused.
        public Question? Question { get; private set; } // TODO: This is unused.

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Reply()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            // Parameterless constructor is required by EF. Will make it private.
        }

        public Reply(string text, string? userId, string? username, DateTime date)
        {
            PozValidate.For.NullOrEmpty(text, nameof(text));

            Text = text;
            UserId = userId;
            UserName = username;
            Date = date;
        }
    }
}
