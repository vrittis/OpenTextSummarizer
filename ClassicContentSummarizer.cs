using System;
using OpenTextSummarizer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OpenTextSummarizer
{
    internal class ClassicContentSummarizer : IContentSummarizer
    {
        public List<string> GetConcepts(AnalyzedDocument analyzedDocument, ISummarizerArguments summarizerArguments)
        {
            if (analyzedDocument.ScoredTextUnits.Count <= summarizerArguments.MaxConceptsInPercent)
            {
                return analyzedDocument.ScoredTextUnits.Select(tus => tus.ScoredTextUnit.FormattedValue).ToList();
            }
            
            double percent = (summarizerArguments.MaxConceptsInPercent / 100D) * analyzedDocument.ScoredTextUnits.Count;
            
            return analyzedDocument.ScoredTextUnits.OrderByDescending(textUnitScore => textUnitScore.Score)
                .Take((int)Math.Round(percent, 0, MidpointRounding.AwayFromZero))
                .Select(textUnitScore => textUnitScore.ScoredTextUnit.FormattedValue)
                .ToList();
        }

        public List<string> GetSentences(AnalyzedDocument analyzedDocument, ISummarizerArguments summarizerArguments)
        {
            var selectedSentences = new List<Sentence>();
            int currentWordCount = 0;
            int currentSentenceIndex = 0;
            int totalContentWordCount = analyzedDocument.ScoredSentences.Sum(s => s.ScoredSentence.TextUnits.Count);
            int targetWordCount = summarizerArguments.MaxSummarySizeInPercent * totalContentWordCount / 100;
            
            while (currentSentenceIndex < analyzedDocument.ScoredSentences.Count - 1 &&
                selectedSentences.Count < summarizerArguments.MaxSummarySentences &&
                currentWordCount < targetWordCount)
            {
                var selectedSentence = analyzedDocument.ScoredSentences[currentSentenceIndex].ScoredSentence;
                selectedSentences.Add(selectedSentence);
                currentWordCount += selectedSentence.TextUnits.Count;
                currentSentenceIndex += 1;
            }

            return selectedSentences.OrderBy(s => s.OriginalSentenceIndex).Select(s => s.OriginalSentence).ToList();
        }
    }
}