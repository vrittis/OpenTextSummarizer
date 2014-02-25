using System.Collections.Generic;

namespace OpenTextSummarizer
{
    public class SummarizedDocument
    {
        public List<string> Concepts { get; set; }

        public List<string> Sentences { get; set; }

        internal SummarizedDocument()
        {
            Sentences = new List<string>();
            Concepts = new List<string>();
        }
    }
}