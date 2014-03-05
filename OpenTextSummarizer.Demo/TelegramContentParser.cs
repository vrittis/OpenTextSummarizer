using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace OpenTextSummarizer.Demo
{
    public class TelegramContentParser : IContentParser
    {
        public List<Sentence> SplitContentIntoSentences(string Content)
        {
            return Content.Split(new string[] { "STOP" }, StringSplitOptions.RemoveEmptyEntries)
                .Select((currentString, currentIndex) => new Sentence() { OriginalSentence = currentString, OriginalSentenceIndex = currentIndex })
                .ToList();
        }

        public List<TextUnit> SplitSentenceIntoTextUnits(string sentence)
        {
            return sentence.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(currentString => new TextUnit() { RawValue = currentString, FormattedValue = currentString.ToLower(), Stem = currentString.ToLower() })
                .ToList();
        }
    }
}