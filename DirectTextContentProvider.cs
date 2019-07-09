using System;
using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer
{
    public class DirectTextContentProvider : IContentProvider
    {
        public string Content { get; }

        public DirectTextContentProvider(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException(nameof(content));
            }
            Content = content;
        }
    }
}