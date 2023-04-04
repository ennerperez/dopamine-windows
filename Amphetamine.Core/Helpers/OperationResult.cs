using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amphetamine.Core.Helpers
{
    public class OperationResult
    {
        private List<string> messages;

        public bool Result { get; set; }

        public OperationResult() => this.messages = new List<string>();

        public void AddMessage(string iMessage) => this.messages.Add(iMessage);

        public string GetFirstMessage() => this.messages.Count > 0 ? this.messages.First<string>() : string.Empty;

        public string GetMessages()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string message in this.messages)
                stringBuilder.AppendLine(message + Environment.NewLine);
            return stringBuilder.ToString();
        }
    }
}