using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    public interface IContentSummarizer
    {
        List<string> GetConcepts(AnalyzedDocument analyzedDocument, SummarizerArguments summarizerArguments);

        List<string> GetSentences(AnalyzedDocument analyzedDocument, SummarizerArguments summarizerArguments);
    }
}