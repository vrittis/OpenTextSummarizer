using System.Diagnostics;

namespace OpenTextSummarizer
{
    [DebuggerDisplay("Value: {FormattedValue} - Stem: {Stem} - Original: {RawValue}")]
    public class TextUnit
    {
        public string RawValue { get; set; }

        public string FormattedValue { get; set; }

        public string Stem { get; set; }

        public override int GetHashCode()
        {
            // TODO: take a look at this  - non-readonly property referenced in GetHashCode()
            return Stem != null ? Stem.GetHashCode() : 0;
        }

        public override bool Equals(object obj)
        {
            return obj is TextUnit other && other.Stem == Stem;
        }
    }
}