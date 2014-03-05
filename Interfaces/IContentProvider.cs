using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    public interface IContentProvider
    {
        string Content { get; }
    }
}