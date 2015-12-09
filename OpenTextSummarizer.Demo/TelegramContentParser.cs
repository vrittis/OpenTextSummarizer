using System;
using System.Collections.Generic;
using System.Linq;
using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer.Demo
{
    public class TelegramContentParser : IContentParser
    {
        public List<Sentence> SplitContentIntoSentences(string Content)
        {
            return Content.Split(new[] {"STOP"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(
                    (currentString, currentIndex) =>
                        new Sentence {OriginalSentence = currentString, OriginalSentenceIndex = currentIndex})
                .ToList();
        }

        public List<TextUnit> SplitSentenceIntoTextUnits(string sentence)
        {
            return sentence.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                .Select(
                    currentString =>
                        new TextUnit
                        {
                            RawValue = currentString,
                            FormattedValue = currentString.ToLower(),
                            Stem = currentString.ToLower()
                        })
                .ToList();
        }
    }
}