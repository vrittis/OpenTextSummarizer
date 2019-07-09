using OpenTextSummarizer.Interfaces;
using System;

namespace OpenTextSummarizer
{
    public class SummarizerArguments : ISummarizerArguments
    {
        public int MaxConceptsInPercent { get; set; }

        public int MaxSummarySentences { get; set; }

        public int MaxSummarySizeInPercent { get; set; }

        public string Language { get; set; }

        private LanguageData _rules;
        private readonly object _rulesLock = new object();

        public SummarizerArguments()
        {
            Language = "en";
            MaxConceptsInPercent = 5;
            MaxSummarySentences = 10;
            MaxSummarySizeInPercent = 10;

            ContentParser = () => new ClassicContentParser(Rules, new TextUnitBuilder(Rules));
            ContentAnalyzer = () => new ClassicContentAnalyzer(Rules);
            ContentSummarizer = () => new ClassicContentSummarizer();
        }

        public LanguageData Rules
        {
            get
            {
                if (_rules == null)
                {
                    lock (_rulesLock)
                    {
                        if (_rules == null)
                        {
                            _rules = LanguageData.LoadFromFile(Language);
                        }
                    }
                }
                return _rules;
            }
        }

        public Func<IContentParser> ContentParser { get; set; }

        public Func<IContentAnalyzer> ContentAnalyzer { get; set; }

        public Func<IContentSummarizer> ContentSummarizer { get; set; }
    }
}