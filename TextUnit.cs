namespace OpenTextSummarizer
{
    public class TextUnit
    {
        public string RawValue { get; set; }

        public string FormattedValue { get; set; }

        public string Stem { get; set; }

        public override int GetHashCode()
        {
            if (Stem == null)
            {
                return 0;
            }
            return Stem.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is TextUnit other && other.Stem == Stem;
        }
    }
}