using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class ClassicContentSummarizer : IContentSummarizer
    {
        public List<string> GetConcepts(AnalyzedDocument analyzedDocument, SummarizerArguments summarizerArguments)
        {
            if (analyzedDocument.ScoredTextUnits.Count <= summarizerArguments.FilteringConceptsCap)
            {
                return analyzedDocument.ScoredTextUnits.Select(tus => tus.ScoredTextUnit.FormattedValue).ToList();
            }

            var baseFrequency = analyzedDocument.ScoredTextUnits[summarizerArguments.FilteringConceptsCap].Score;
            return analyzedDocument.ScoredTextUnits.Where(tus => tus.Score >= baseFrequency).Select(tus => tus.ScoredTextUnit.FormattedValue).ToList();
        }

        public List<string> GetSentences(AnalyzedDocument analyzedDocument, SummarizerArguments summarizerArguments)
        {
            var totalContentWordCount = analyzedDocument.ScoredSentences.Sum(s => s.ScoredSentence.TextUnits.Count);
            var targetWordCount = summarizerArguments.LowerPercentageOfInitialContentCap * totalContentWordCount / 100;
            var currentWordCount = 0;
            var currentSentenceCount = 0;

            var selectedSentences = new List<Sentence>();

            var currentSentenceIndex = 0;
            while (currentSentenceIndex < analyzedDocument.ScoredSentences.Count - 1 && currentWordCount < targetWordCount && currentSentenceCount < summarizerArguments.LowerSentenceNumberCap)
            {
                selectedSentences.Add(analyzedDocument.ScoredSentences[currentSentenceIndex].ScoredSentence);
            }

            return selectedSentences.OrderBy(s => s.OriginalSentenceIndex).Select(s => s.OriginalSentence).ToList();
        }
    }
}