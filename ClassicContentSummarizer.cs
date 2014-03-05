using OpenTextSummarizer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OpenTextSummarizer
{
    internal class ClassicContentSummarizer : IContentSummarizer
    {
        public List<string> GetConcepts(AnalyzedDocument analyzedDocument, ISummarizerArguments summarizerArguments)
        {
            if (analyzedDocument.ScoredTextUnits.Count <= summarizerArguments.FilteringConceptsCap)
            {
                return analyzedDocument.ScoredTextUnits.Select(tus => tus.ScoredTextUnit.FormattedValue).ToList();
            }

            var baseFrequency = analyzedDocument.ScoredTextUnits[summarizerArguments.FilteringConceptsCap].Score;
            return analyzedDocument.ScoredTextUnits.Where(tus => tus.Score >= baseFrequency).Select(tus => tus.ScoredTextUnit.FormattedValue).ToList();
        }

        public List<string> GetSentences(AnalyzedDocument analyzedDocument, ISummarizerArguments summarizerArguments)
        {
            var totalContentWordCount = analyzedDocument.ScoredSentences.Sum(s => s.ScoredSentence.TextUnits.Count);
            var targetWordCount = summarizerArguments.MaxSummarySizeInPercent * totalContentWordCount / 100;
            var currentWordCount = 0;
            var currentSentenceIndex = 0;
            var selectedSentences = new List<Sentence>();

            while (currentSentenceIndex < analyzedDocument.ScoredSentences.Count - 1 &&
                    selectedSentences.Count < summarizerArguments.MaxSummarySentences &&
                    currentWordCount < targetWordCount)
            {
                var selectedSentence = analyzedDocument.ScoredSentences[currentSentenceIndex].ScoredSentence;
                selectedSentences.Add(selectedSentence);
                currentWordCount += selectedSentence.TextUnits.Count();
                currentSentenceIndex += 1;
            }

            return selectedSentences.OrderBy(s => s.OriginalSentenceIndex).Select(s => s.OriginalSentence).ToList();
        }
    }
}