using OpenTextSummarizer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OpenTextSummarizer
{
    /// <summary>
    /// Analyzer class to determine important text units and score sentences
    /// </summary>
    internal class ClassicContentAnalyzer : IContentAnalyzer
    {
        public LanguageData Rules { get; set; }

        public ClassicContentAnalyzer(LanguageData rules)
        {
            Rules = rules;
        }

        public List<TextUnitScore> GetImportantTextUnits(List<Sentence> sentences)
        {
            var textUnitFrequencyGrader = new Dictionary<TextUnit, long>();
            foreach (var tu in sentences.SelectMany(s => s.TextUnits))
            {
                if (Rules.UnimportantWords.Contains(tu.FormattedValue))
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

            return textUnitFrequencyGrader.OrderByDescending(kvp => kvp.Value)
                .Select(kvp => new TextUnitScore { ScoredTextUnit = kvp.Key, Score = kvp.Value })
                .ToList();
        }

        public List<SentenceScore> ScoreSentences(List<Sentence> sentences, List<TextUnitScore> importantTextUnits)
        {
            var stemList = importantTextUnits.Select(tus => tus.ScoredTextUnit.Stem).Distinct().ToList();
            var listSentenceScorer = new List<SentenceScore>();
            foreach (var sentence in sentences.Where(s => s.TextUnits.Count > 2))
            {
                var newSentenceScorer = new SentenceScore { ScoredSentence = sentence };
                newSentenceScorer.Score = newSentenceScorer.ScoredSentence.TextUnits.Count(tu => stemList.Contains(tu.Stem));

                if (sentence.TextUnits[0].RawValue.Contains("\n") && sentence.TextUnits[1].RawValue.Contains("\n"))
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

            return listSentenceScorer.OrderByDescending(sentenceScore => sentenceScore.Score).ToList();
        }
    }
}