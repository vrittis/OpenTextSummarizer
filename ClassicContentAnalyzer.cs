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
            var textUnitFrequencyGrader = new Dictionary<TextUnit, int>();
            foreach (var tu in sentences.Where(sentence => sentence.TextUnits.Count > 0).SelectMany(s => s.TextUnits))
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
            var stemList = importantTextUnits.Select(tus => tus.ScoredTextUnit.Stem).Distinct().ToArray();
            var listSentenceScorer = new List<SentenceScore>();
            foreach (var sentence in sentences.Where(sentence => sentence.TextUnits.Count > 2))
            {
                var newSentenceScorer = CalculateSentenceScore(sentence, stemList);
                
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

        private SentenceScore CalculateSentenceScore(Sentence sentence, IEnumerable<string> stemList)
        {
            return new SentenceScore
            {
                ScoredSentence = sentence,
                Score = sentence.TextUnits.Count(textUnit => stemList.Contains(textUnit.Stem))
            };
        }
    }
}