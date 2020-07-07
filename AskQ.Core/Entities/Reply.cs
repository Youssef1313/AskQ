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

        public int QuestionId { get; private set; }
        public Question? Question { get; private set; }

        private Reply()
        {
            // Parameterless constructor is required by EF. Will make it private.
        }
        public Reply(string text, string? userId, string? username)
        {
            PozValidate.For.NullOrEmpty(text, nameof(text));

            this.Text = text;
            this.UserId = userId;
            this.UserName = username;
        }
    }
}
