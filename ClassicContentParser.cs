using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer
{
    internal class ClassicContentParser : IContentParser
    {
        public ClassicContentParser(Dictionary rules, ITextUnitBuilder textUnitBuilder)
        {
            m_Rules = rules;
            m_TextUnitBuilder = textUnitBuilder;
        }

        internal Dictionary m_Rules { get; set; }

        public ITextUnitBuilder m_TextUnitBuilder { get; set; }

        public List<Sentence> SplitContentIntoSentences(string Content)
        {
            var listSentences = new List<Sentence>();
            if (string.IsNullOrEmpty(Content))
            {
                return listSentences;
            }

            var words = Content.Split(' ', '\r'); //space and line feed characters are the ends of words.
            var cursentence = new Sentence {OriginalSentenceIndex = listSentences.Count};
            listSentences.Add(cursentence);
            var originalSentence = new StringBuilder();
            foreach (var word in words)
            {
                var locWord = word;
                if (locWord.StartsWith("\n") && word.Length > 2) locWord = locWord.Replace("\n", "");

                if (IsSentenceBreak(locWord))
                {
                    originalSentence.AppendFormat("{0}", locWord);
                    cursentence.OriginalSentence = originalSentence.ToString();
                    cursentence = new Sentence {OriginalSentenceIndex = listSentences.Count};
                    originalSentence = new StringBuilder();
                    listSentences.Add(cursentence);
                }
                else
                {
                    originalSentence.AppendFormat("{0} ", locWord);
                }
            }
            cursentence.OriginalSentence = originalSentence.ToString();
            return listSentences;
        }

        public List<TextUnit> SplitSentenceIntoTextUnits(string sentence)
        {
            var listUnits = new List<TextUnit>();
            if (string.IsNullOrEmpty(sentence))
            {
                return listUnits;
            }

            listUnits.AddRange(sentence.Split(' ', '\r').Select(word => m_TextUnitBuilder.Build(word)));

            return listUnits;
        }

        private bool IsSentenceBreak(string word)
        {
            if (word.Contains("\r") || word.Contains("\n")) return true;
            var shouldBreak =
                m_Rules.LinebreakRules.Any(p => word.EndsWith(p, StringComparison.CurrentCultureIgnoreCase));

            if (shouldBreak == false) return false;

            shouldBreak =
                !m_Rules.NotALinebreakRules.Any(p => word.StartsWith(p, StringComparison.CurrentCultureIgnoreCase));

            return shouldBreak;
        }
    }
}