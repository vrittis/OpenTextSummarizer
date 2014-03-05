using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    public interface IContentAnalyzer
    {
        List<TextUnitScore> GetImportantTextUnits(List<Sentence> Sentences);

        List<SentenceScore> ScoreSentences(List<Sentence> Sentences, List<TextUnitScore> importantTextUnits);
    }
}