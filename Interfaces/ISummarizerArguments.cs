using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    /// <summary>
    /// Interface for summarizing arguments
    /// </summary>
    public interface ISummarizerArguments
    {
        /// <summary>
        /// Concepts are kept depending on their frequency. Setting this value to 5 ensures that you
        /// will have at least 5 concepts in the results (unless there is not even 5 concepts); you
        /// may however have more than 5 concepts if some concepts have the same frequency as the
        /// fifth concept in the list.
        /// </summary>
        int FilteringConceptsCap { get; set; }

        /// <summary>
        /// Sets the maximum number of sentences to return in the summary; the upper limit is
        /// determined by the minimum of sentences this value and MaxSummarySizeInPercent will let
        /// pass. If negative this value will be set to 0
        /// </summary>
        int MaxSummarySentences { get; set; }

        /// <summary>
        /// Sets the maximum size in percentage of the original text; the summarizer will finish any
        /// sentence that makes the summary go over this value; the upper limit is determined by the
        /// minimum of sentences this value and MaxSummarySentences will let pass. This parameter
        /// will be adjusted in the 0-100 range if set to a value outside this range.
        /// </summary>
        int MaxSummarySizeInPercent { get; set; }

        /// <summary>
        /// Sets the content parser factory: plug in your own implementation of a parser to change
        /// the behavior of the summarizer
        /// </summary>
        Func<IContentParser> ContentParser { get; set; }

        /// <summary>
        /// Sets the content analyzer factory: plug in your own implementation of an analyzer to
        /// change the behavior of the summarizer
        /// </summary>
        Func<IContentAnalyzer> ContentAnalyzer { get; set; }

        /// <summary>
        /// Sets the content summarizer factory: plug in your own implementation of a summarizer to
        /// change the behavior of the summarizer
        /// </summary>
        Func<IContentSummarizer> ContentSummarizer { get; set; }
    }
}