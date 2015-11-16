namespace OpenTextSummarizer
{
    public class TextUnit
    {
        public string RawValue { get; set; }

        public string FormattedValue { get; set; }

        public string Stem { get; set; }

        public override int GetHashCode()
        {
            return Stem?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TextUnit;
            return other != null && other.Stem == Stem;
        }
    }
}