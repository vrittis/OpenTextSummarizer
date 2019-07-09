using System.Collections.Generic;

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
        /// <param name="content">The content that needs to be split into sentences</param>
        /// <returns>List of sentences that compose the content</returns>
        List<Sentence> SplitContentIntoSentences(string content);

        /// <summary>
        /// Returns a sentence as 
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        List<TextUnit> SplitSentenceIntoTextUnits(string sentence);
    }
}