using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    /// <summary>
    /// Interface to the formatting and stemming of textual units
    /// </summary>
    internal interface ITextUnitBuilder
    {
        /// <summary>
        /// Returns a TextUnit built from a word. Formatted values are the basis for comparison
        /// between words
        /// </summary>
        /// <param name="word">The word you want to use as a TextUnit</param>
        /// <returns>TextUnit built from the word</returns>
        TextUnit Build(string word);
    }
}