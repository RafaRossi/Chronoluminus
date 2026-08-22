using System.Collections.Generic;

namespace DialogueSystem
{
    public readonly struct DisplayLine
    {
        public readonly string Text;
        public readonly string SpeakerName;
        public readonly string SpriteKey;

        public DisplayLine(string text, string speakerName, string spriteKey)
        {
            Text = text;
            SpeakerName = speakerName;
            SpriteKey = spriteKey;
        }
    }

    public static class DialogueLineParser
    {
        public static DisplayLine Parse(string text, List<string> tags)
        {
            string speaker = null;
            string sprite = null;

            foreach (var tag in tags)
            {
                var parts = tag.Split(':', 2);
                if (parts.Length != 2) continue;

                switch (parts[0].Trim().ToLowerInvariant())
                {
                    case "speaker": speaker = parts[1].Trim(); break;
                    case "sprite": sprite = parts[1].Trim(); break;
                }
            }

            return new DisplayLine(text, speaker, sprite);
        }
    }
}