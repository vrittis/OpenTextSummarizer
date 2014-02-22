using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class Article
    {
        public List<Sentence> Sentences { get; set; }
        public int LineCount { get; set; }
        public List<string> Concepts { get; set; }
        public Dictionary Rules { get; set; }

        public List<Word> ImportantWords { get; set; }
        public List<Word> WordCounts { get; set; }


        public Article(Dictionary rules) { 
            Sentences = new List<Sentence>();
            WordCounts = new List<Word>();
            Concepts = new List<string>();
            Rules = rules;
        }

        public void ParseText(string text)
        {
            string[] words = text.Split(' ', '\r'); //space and line feed characters are the ends of words.
            Sentence cursentence = new Sentence();
            this.Sentences.Add(cursentence);
            StringBuilder originalSentence = new StringBuilder();
            foreach (string word in words)
            {
                string locWord = word;
                if (locWord.StartsWith("\n") && word.Length > 2) locWord = locWord.Replace("\n", "");
                originalSentence.AppendFormat("{0} ", locWord);
                cursentence.Words.Add(new Word(locWord));
                AddWordCount(locWord);
                if (IsSentenceBreak(locWord))
                {
                    cursentence.OriginalSentence = originalSentence.ToString();
                    cursentence = new Sentence();
                    originalSentence = new StringBuilder();
                    this.Sentences.Add(cursentence);
                }
            }
        }

        private void AddWordCount(string word)
        {
            Word stemmedWord = Stemmer.StemWord(word, this.Rules);
            if (word == null || word == string.Empty || word == " " || word == "\n" || word == "\t") return;            
            Word foundWord = WordCounts.Find(delegate(Word match) {
                return match.Stem == stemmedWord.Stem;
            });
            if (foundWord == null)
            {
                WordCounts.Add(stemmedWord);
            }
            else
            {
                foundWord.TermFrequency++;
            }

        }

        private bool IsSentenceBreak(string word)
        {
            if (word.Contains("\r") || word.Contains("\n")) return true;
            bool shouldBreak = (this.Rules.LinebreakRules
                .Where(p => word.EndsWith(p, StringComparison.CurrentCultureIgnoreCase))
                .Count() > 0);

            

            if (shouldBreak == false) return shouldBreak;

            shouldBreak = (this.Rules.NotALinebreakRules
                .Where(p => word.StartsWith(p, StringComparison.CurrentCultureIgnoreCase))
                .Count() == 0);


            return shouldBreak;
        }

    }
}
