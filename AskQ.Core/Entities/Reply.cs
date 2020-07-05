using System;

namespace AskQ.Core.Entities
{
    public class Reply
    {
        public int Id { get; set; }
        public Question Question { get; set; } = null!;
        public string ReplyText { get; set; } = null!;
        public string? UserGuid { get; set; } // Nullable because I'm allowing annonymous.
        public DateTime DateTime { get; set; }
    }
}
