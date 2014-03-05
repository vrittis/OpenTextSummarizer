using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class ClassicContentParser : IContentParser
    {
        internal Dictionary m_Rules { get; set; }

        public ITextUnitBuilder m_TextUnitBuilder { get; set; }

        public ClassicContentParser(Dictionary rules, ITextUnitBuilder textUnitBuilder)
        {
            m_Rules = rules;
            m_TextUnitBuilder = textUnitBuilder;
        }

        public List<Sentence> SplitContentIntoSentences(string Content)
        {
            //TODO: string checking

            var listSentences = new List<Sentence>();

            string[] words = Content.Split(' ', '\r'); //space and line feed characters are the ends of words.
            Sentence cursentence = new Sentence() { OriginalSentenceIndex = listSentences.Count };
            listSentences.Add(cursentence);
            StringBuilder originalSentence = new StringBuilder();
            foreach (string word in words)
            {
                string locWord = word;
                if (locWord.StartsWith("\n") && word.Length > 2) locWord = locWord.Replace("\n", "");
                originalSentence.AppendFormat("{0} ", locWord);

                if (IsSentenceBreak(locWord))
                {
                    cursentence.OriginalSentence = originalSentence.ToString();
                    cursentence = new Sentence() { OriginalSentenceIndex = listSentences.Count };
                    originalSentence = new StringBuilder();
                    listSentences.Add(cursentence);
                }
            }
            cursentence.OriginalSentence = originalSentence.ToString();
            return listSentences;
        }

        private bool IsSentenceBreak(string word)
        {
            if (word.Contains("\r") || word.Contains("\n")) return true;
            bool shouldBreak = (m_Rules.LinebreakRules
                .Where(p => word.EndsWith(p, StringComparison.CurrentCultureIgnoreCase))
                .Count() > 0);

            if (shouldBreak == false) return shouldBreak;

            shouldBreak = (m_Rules.NotALinebreakRules
                .Where(p => word.StartsWith(p, StringComparison.CurrentCultureIgnoreCase))
                .Count() == 0);

            return shouldBreak;
        }

        public List<TextUnit> SplitSentenceIntoTextUnits(string sentence)
        {
            //TODO: string checking

            var listUnits = new List<TextUnit>();

            foreach (string word in sentence.Split(' ', '\r'))
            {
                listUnits.Add(m_TextUnitBuilder.Build(word));
            }

            return listUnits;
        }
    }
}