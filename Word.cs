using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class Word
    {
        public string Value { get; set; }
        public string Stem { get; set; }
        public double TermFrequency { get; set; }

        public Word() { }
        public Word(string word) { Value = word; }

        public override bool Equals(object obj)
        {
            if (this.GetType() != obj.GetType()) return false;
            Word arg = (Word)obj;
            return this.Value.Equals(arg.Value, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
