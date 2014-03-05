using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer.Interfaces
{
    internal interface ITextUnitBuilder
    {
        TextUnit Build(string word);
    }
}