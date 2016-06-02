using System;
using System.Linq;

namespace CoreMUD.Core
{
    public class ActionInput
    {
        public ActionInput(string fullText, object controller)
        {
            Controller = controller;
            parseCommand(fullText);
        }

        private void parseCommand(string fullText)
        {
            var parts = fullText?.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Any())
            {
                FullText = string.Join(" ", parts);
                Noun = parts[0].ToLower();
                if (parts.Length > 1)
                {
                    Params = parts.Skip(1).ToArray();
                    Tail = string.Join(" ", Params);
                }
            }

        }

        public string Noun { get; set; }
        public string[] Params { get; set; }
        public object Tail { get; set; }
        public object Controller { get; private set; }
        public string FullText { get; private set; }
    }
}
