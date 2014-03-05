using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    /// <summary>
    /// Content summarizer interface: implement to define parsing behavior
    /// </summary>
    public interface IContentSummarizer
    {
        /// <summary>
        /// Returns the concepts that can be extracted from the AnalyzedDocuments and the summarizer arguments
        /// </summary>
        /// <param name="analyzedDocument">
        /// The document containing the scored sentences and text units
        /// </param>
        /// <param name="summarizerArguments">Parameters for summary extraction</param>
        /// <returns>A list of string representing the most important concepts</returns>
        List<string> GetConcepts(AnalyzedDocument analyzedDocument, ISummarizerArguments summarizerArguments);

        /// <summary>
        /// Returns the sentences that can be extracted from the AnalyzedDocuments and the
        /// summarizer arguments
        /// </summary>
        /// <param name="analyzedDocument">
        /// The document containing the scored sentences and text units
        /// </param>
        /// <param name="summarizerArguments">Parameters for summary extraction</param>
        /// <returns>A list of string representing the most important sentences</returns>
        List<string> GetSentences(AnalyzedDocument analyzedDocument, ISummarizerArguments summarizerArguments);
    }
}