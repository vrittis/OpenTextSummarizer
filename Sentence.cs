using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class Sentence
    {
        public List<Word> Words { get; set; }
        public double Score { get; set; }
        public bool Selected { get; set; }
        public int WordCount { get; set; }
        public string OriginalSentence { get; set; }

        public Sentence() { 
            Words = new List<Word>();
            Selected = false;
        }
        
    }
}
