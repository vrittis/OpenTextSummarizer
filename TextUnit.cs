namespace OpenTextSummarizer
{
    public class TextUnit
    {
        public string RawValue { get; set; }

        public string FormattedValue { get; set; }

        public string Stem { get; set; }

        public override int GetHashCode()
        {
            if (Stem == null) return 0;
            return Stem.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TextUnit other = obj as TextUnit;
            return other != null && other.Stem == this.Stem;
        }
    }
}