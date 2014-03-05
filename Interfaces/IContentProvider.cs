using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    /// <summary>
    /// Content provider interface: implement to define content providing behavior
    /// </summary>
    public interface IContentProvider
    {
        /// <summary>
        /// Returns the content that will be used for a summary
        /// </summary>
        string Content { get; }
    }
}