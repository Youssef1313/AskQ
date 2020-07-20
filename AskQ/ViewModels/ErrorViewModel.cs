using System;

namespace AskQ.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string requestId) =>
            RequestId = requestId ?? throw new ArgumentNullException(nameof(requestId));

        public string RequestId { get; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
