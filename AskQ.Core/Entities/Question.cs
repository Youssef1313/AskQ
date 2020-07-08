using System;
using System.Collections.Generic;
using System.Linq;
using PozitronDev.Validations;

namespace AskQ.Core.Entities
{
    public class Question
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public string AskedToUserId { get; private set; }
        public string AskedToUsername { get; private set; }
        public string? AskedFromUserId { get; private set; }
        public string? AskedFromUsername { get; private set; }
        public DateTime Date { get; private set; }
        public bool HasReplies { get; private set; }

        public IEnumerable<Reply> Replies => _replies.AsEnumerable();
        private readonly List<Reply> _replies = new List<Reply>();

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Question()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            // Parameterless constructor is required by EF. Will make it private.
        }

        public Question(string text, string askedToUserId, string askedToUsername, string? askedFromUserId = null, string? askedFromUsername = null)
        {
            PozValidate.For.NullOrEmpty(text, nameof(text));
            PozValidate.For.NullOrEmpty(askedToUserId, nameof(askedToUserId));
            PozValidate.For.NullOrEmpty(askedToUsername, nameof(askedToUsername));

            Text = text;
            AskedToUserId = askedToUserId;
            AskedToUsername = askedToUsername;
            AskedFromUserId = askedFromUserId;
            AskedFromUsername = askedFromUsername;

            Date = DateTime.UtcNow;
            HasReplies = false;
        }

        public Reply GetReply(int replyId)
        {
            Reply? reply = Replies.FirstOrDefault(x => x.Id == replyId);
            PozValidate.For.NotFound(replyId, reply, nameof(reply));

            return reply;
        }

        public Reply AddReply(Reply reply)
        {
            PozValidate.For.Null(reply, nameof(reply));

            _replies.Add(reply);
            HasReplies = true;

            return reply;
        }

        public void DeleteReply(int replyId)
        {
            Reply reply = GetReply(replyId);
            _replies.Remove(reply);

            if (!Replies.Any())
            {
                HasReplies = false;
            }
        }
    }
}
