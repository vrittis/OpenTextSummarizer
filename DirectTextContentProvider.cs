using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    public class DirectTextContentProvider : IContentProvider
    {
        public string Content { get; private set; }

        public DirectTextContentProvider(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }
            Content = content;
        }
    }
}