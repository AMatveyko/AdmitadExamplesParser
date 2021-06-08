// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    internal sealed class Message
    {
        public Message(
            bool important,
            DateTime time,
            string text )
        {
            Important = important;
            Time = time;
            Text = text;
        }

        public bool Important { get; }
        public DateTime Time { get; }
        public string Text { get; }

        public override string ToString()
        {
            return $"{Time.ToString()} {Text}";
        }
    }
}