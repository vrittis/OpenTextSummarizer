using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer
{
    internal class ClassicContentParser : IContentParser
    {
        internal LanguageData Rules { get; set; }

        public ITextUnitBuilder TextUnitBuilder { get; set; }

        public ClassicContentParser(LanguageData rules, ITextUnitBuilder textUnitBuilder)
        {
            Rules = rules;
            TextUnitBuilder = textUnitBuilder;
        }

        public List<Sentence> SplitContentIntoSentences(string content)
        {
            var listSentences = new List<Sentence>();
            if (string.IsNullOrEmpty(content))
            {
                return listSentences;
            }

            string[] words = content.Split(' ', '\r'); //space and line feed characters are the ends of words
            Sentence currentSentence = new Sentence { OriginalSentenceIndex = listSentences.Count };
            listSentences.Add(currentSentence);
            var originalSentence = new StringBuilder();
            foreach (string word in words)
            {
                string locWord = word;
                if (locWord.StartsWith("\n") && word.Length > 2)
                {
                    locWord = locWord.Replace("\n", string.Empty);
                }

                if (IsSentenceBreak(locWord))
                {
                    originalSentence.Append(locWord.Trim());
                    currentSentence.OriginalSentence = originalSentence.ToString();
                    currentSentence = new Sentence { OriginalSentenceIndex = listSentences.Count };
                    originalSentence = new StringBuilder();
                    listSentences.Add(currentSentence);
                }
                else
                {
                    locWord = locWord.Trim();
                    originalSentence.Append($"{locWord} ");
                }
            }
            currentSentence.OriginalSentence = originalSentence.ToString();
            return listSentences;
        }

        private bool IsSentenceBreak(string word)
        {
            if (word.Contains("\r") || word.Contains("\n"))
            {
                return true;
            }

            bool shouldBreak = Rules.LinebreakRules.Any(s => word.EndsWith(s, StringComparison.CurrentCultureIgnoreCase));
            if (shouldBreak == false)
            {
                return false;
            }

            shouldBreak = !Rules.NotALinebreakRules.Any(s => word.StartsWith(s, StringComparison.CurrentCultureIgnoreCase));
            return shouldBreak;
        }

        public List<TextUnit> SplitSentenceIntoTextUnits(string sentence)
        {
            return string.IsNullOrEmpty(sentence)
                ? new List<TextUnit>()
                : sentence.Split(' ', '\r')
                    .Select(word => TextUnitBuilder.Build(word))
                    .ToList();
        }
    }
}