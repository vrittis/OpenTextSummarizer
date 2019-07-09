using System.Collections.Generic;

namespace OpenTextSummarizer.Interfaces
{
    /// <summary>
    /// Analysis interface: implement to define analysis behavior
    /// </summary>
    public interface IContentAnalyzer
    {
        /// <summary>
        /// Return significant text units with their importance score (eg frequency of the text unit)
        /// </summary>
        /// <param name="sentences">List of sentences to analyze</param>
        /// <returns>
        /// List of text units scored according to their importance. Sorting the items is not
        /// necessary since the engine sorts the returned list
        /// </returns>
        List<TextUnitScore> GetImportantTextUnits(List<Sentence> sentences);

        /// <summary>
        /// Return significant sentences with their importance score (eg number of important text
        /// units in the sentence)
        /// </summary>
        /// <param name="sentences">List of sentences to analyse</param>
        /// <param name="importantTextUnits">List of text units with their importance score</param>
        /// <returns>
        /// List of sentences scored according to their importance. Sorting the sentences is not
        /// necessary since the engine sorts the returned list
        /// </returns>
        List<SentenceScore> ScoreSentences(List<Sentence> sentences, List<TextUnitScore> importantTextUnits);
    }
}