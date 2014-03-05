using OpenTextSummarizer;
using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    public class SummarizerArguments : ISummarizerArguments
    {
        public int FilteringConceptsCap { get; set; }

        public int LowerSentenceNumberCap { get; set; }

        public int LowerPercentageOfInitialContentCap { get; set; }

        public string Language { get; set; }

        public SummarizerArguments()
        {
            Language = "en";
            FilteringConceptsCap = 5;
            LowerSentenceNumberCap = 10;
            LowerPercentageOfInitialContentCap = 10;

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