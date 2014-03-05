using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    /// <summary>
    /// Analyzer class to determine important text units and score sentences
    /// </summary>
    internal class ClassicContentAnalyzer : IContentAnalyzer
    {
        public Dictionary m_Rules { get; set; }

        public ClassicContentAnalyzer(Dictionary Rules)
        {
            m_Rules = Rules;
        }

        public List<TextUnitScore> GetImportantTextUnits(List<Sentence> Sentences)
        {
            var textUnitFrequencyGrader = new Dictionary<TextUnit, long>();
            foreach (var tu in Sentences.SelectMany(s => s.TextUnits))
            {
                if (m_Rules.UnimportantWords.Contains(tu.FormattedValue))
                {
                    continue;
                }

                if (textUnitFrequencyGrader.ContainsKey(tu))
                {
                    textUnitFrequencyGrader[tu]++;
                }
                else
                {
                    textUnitFrequencyGrader.Add(tu, 1);
                }
            }

            return textUnitFrequencyGrader.OrderByDescending(kvp => kvp.Value).Select(kvp => new TextUnitScore() { ScoredTextUnit = kvp.Key, Score = kvp.Value }).ToList();
        }

        public List<SentenceScore> ScoreSentences(List<Sentence> Sentences, List<TextUnitScore> importantTextUnits)
        {
            var stemList = importantTextUnits.Select(tus => tus.ScoredTextUnit.Stem).Distinct().ToList();
            var listSentenceScorer = new List<SentenceScore>();
            foreach (var s in Sentences.Where(s => s.TextUnits.Count > 2))
            {
                var newSentenceScorer = new SentenceScore();
                newSentenceScorer.ScoredSentence = s;
                newSentenceScorer.Score = newSentenceScorer.ScoredSentence.TextUnits.Count(tu => stemList.Contains(tu.Stem));

                if (s.TextUnits[0].RawValue.Contains("\n") && s.TextUnits[1].RawValue.Contains("\n"))
                {
                    newSentenceScorer.Score *= 1.6;
                }

                listSentenceScorer.Add(newSentenceScorer);
            }

            // additional scoring
            if (listSentenceScorer.Any())
            {
                listSentenceScorer.First().Score *= 2;
            }

            return listSentenceScorer.OrderByDescending(ss => ss.Score).ToList();
        }
    }
}