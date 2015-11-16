using System.Collections.Generic;

namespace OpenTextSummarizer
{
    public class AnalyzedDocument
    {
        public List<TextUnitScore> ScoredTextUnits { get; set; }

        public List<SentenceScore> ScoredSentences { get; set; }
    }
}