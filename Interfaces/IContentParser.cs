using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    /// <summary>
    /// Parsing interface: implement to define parsing behavior
    /// </summary>
    public interface IContentParser
    {
        /// <summary>
        /// Returns content as list of sentences
        /// </summary>
        /// <param name="Content">The content that needs to be split into sentences</param>
        /// <returns>List of sentences that compose the content</returns>
        List<Sentence> SplitContentIntoSentences(string Content);

        /// <summary>
        /// Returns a sentence as 
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        List<TextUnit> SplitSentenceIntoTextUnits(string sentence);
    }
}