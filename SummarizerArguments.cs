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

        private Dictionary m_Rules = null;
        private object m_RulesLock = new object();

        internal Dictionary Rules
        {
            get
            {
                if (m_Rules == null)
                {
                    lock (m_RulesLock)
                    {
                        if (m_Rules == null)
                        {
                            m_Rules = Dictionary.LoadFromFile(Language);
                        }
                    }
                }
                return m_Rules;
            }
        }

        public Func<IContentParser> ContentParser { get; set; }

        public Func<IContentAnalyzer> ContentAnalyzer { get; set; }

        public Func<IContentSummarizer> ContentSummarizer { get; set; }
    }
}