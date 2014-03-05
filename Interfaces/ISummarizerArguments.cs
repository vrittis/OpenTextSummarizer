using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    public interface ISummarizerArguments
    {
        int FilteringConceptsCap { get; set; }

        int LowerSentenceNumberCap { get; set; }

        int LowerPercentageOfInitialContentCap { get; set; }
    }
}