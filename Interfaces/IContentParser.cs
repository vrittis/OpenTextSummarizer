using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    public interface IContentParser
    {
        List<Sentence> SplitContentIntoSentences(string Content);

        List<TextUnit> SplitSentenceIntoTextUnits(string sentence);
    }
}