using OpenTextSummarizer.Interfaces;
using System;

namespace OpenTextSummarizer
{
    public class SummarizerArguments : ISummarizerArguments
    {
        public int FilteringConceptsCap { get; set; }

        public int MaxSummarySentences { get; set; }

        public int MaxSummarySizeInPercent { get; set; }

        public string Language { get; set; }

        public SummarizerArguments()
        {
            Language = "en";
            FilteringConceptsCap = 5;
            MaxSummarySentences = 10;
            MaxSummarySizeInPercent = 10;

            ContentParser = () => new ClassicContentParser(Rules, new TextUnitBuilder(Rules));
            ContentAnalyzer = () => new ClassicContentAnalyzer(Rules);
            ContentSummarizer = () => new ClassicContentSummarizer();
        }

        private Dictionary _mRules;
        private object _mRulesLock = new object();

        internal Dictionary Rules
        {
            get
            {
                if (_mRules == null)
                {
                    lock (_mRulesLock)
                    {
                        if (_mRules == null)
                        {
                            _mRules = Dictionary.LoadFromFile(Language);
                        }
                    }
                }
                return _mRules;
            }
        }

        public Func<IContentParser> ContentParser { get; set; }

        public Func<IContentAnalyzer> ContentAnalyzer { get; set; }

        public Func<IContentSummarizer> ContentSummarizer { get; set; }
    }
}