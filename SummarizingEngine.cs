using OpenTextSummarizer.Interfaces;
using System;
using System.Linq;

namespace OpenTextSummarizer
{
    /// <summary>
    /// Orchestrates the different parts of the summarizing algorithm
    /// </summary>
    internal class SummarizingEngine
    {
        /// <summary>
        /// Runs the content parsing part of the summarizing algorithm
        /// </summary>
        /// <param name="contentProvider"></param>
        /// <param name="contentParser"></param>
        /// <returns></returns>
        public ParsedDocument ParseContent(IContentProvider contentProvider, IContentParser contentParser)
        {
            if (contentProvider == null)
            {
                throw new ArgumentNullException("contentProvider");
            }
            if (contentParser == null)
            {
                throw new ArgumentNullException("contentParser");
            }

            var resultingParsedDocument = new ParsedDocument();
            resultingParsedDocument.Sentences = contentParser.SplitContentIntoSentences(contentProvider.Content);
            if (resultingParsedDocument.Sentences == null)
            {
                throw new InvalidOperationException(string.Format("{0}.SplitContentIntoSentences must not return null", contentProvider.GetType().FullName));
            }
            foreach (var workingSentence in resultingParsedDocument.Sentences)
            {
                workingSentence.TextUnits = contentParser.SplitSentenceIntoTextUnits(workingSentence.OriginalSentence);
                if (workingSentence.TextUnits == null)
                {
                    throw new InvalidOperationException(string.Format("{0}.SplitSentenceIntoTextUnits must not return null", contentProvider.GetType().FullName));
                }
            }
            return resultingParsedDocument;
        }

        /// <summary>
        /// Runs the content analyzis part of the summarizing algorithm
        /// </summary>
        /// <param name="parsedDocument"></param>
        /// <param name="contentAnalyzer"></param>
        /// <returns></returns>
        public AnalyzedDocument AnalyzeParsedContent(ParsedDocument parsedDocument, IContentAnalyzer contentAnalyzer)
        {
            if (parsedDocument == null)
            {
                throw new ArgumentNullException("parsedDocument");
            }
            if (contentAnalyzer == null)
            {
                throw new ArgumentNullException("contentAnalyzer");
            }

            var importantTextUnits = contentAnalyzer.GetImportantTextUnits(parsedDocument.Sentences);
            if (importantTextUnits == null)
            {
                throw new InvalidOperationException(string.Format("{0}.GetImportantTextUnits must not return null", contentAnalyzer.GetType().FullName));
            }
            var scoredSentences = contentAnalyzer.ScoreSentences(parsedDocument.Sentences, importantTextUnits);
            if (scoredSentences == null)
            {
                throw new InvalidOperationException(string.Format("{0}.ScoreSentences must not return null", contentAnalyzer.GetType().FullName));
            }

            return new AnalyzedDocument() { ScoredTextUnits = importantTextUnits.OrderByDescending(tus => tus.Score).ToList(), ScoredSentences = scoredSentences.OrderByDescending(ss => ss.Score).ToList() };
        }

        /// <summary>
        /// Runs the content summarizing part of the summarizing algorithm
        /// </summary>
        /// <param name="analyzedDocument"></param>
        /// <param name="contentSummarizer"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public SummarizedDocument SummarizeAnalysedContent(AnalyzedDocument analyzedDocument, IContentSummarizer contentSummarizer, ISummarizerArguments arguments)
        {
            if (analyzedDocument == null)
            {
                throw new ArgumentNullException("analyzedDocument");
            }

            if (contentSummarizer == null)
            {
                throw new ArgumentNullException("contentSummarizer");
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            // Range adjustment
            if (arguments.FilteringConceptsCap < 0)
            {
                arguments.FilteringConceptsCap = 0;
            }

            if (arguments.MaxSummarySentences < 0)
            {
                arguments.MaxSummarySentences = 0;
            }

            if (arguments.MaxSummarySizeInPercent < 0)
            {
                arguments.MaxSummarySizeInPercent = 0;
            }

            if (arguments.MaxSummarySizeInPercent > 100)
            {
                arguments.MaxSummarySizeInPercent = 100;
            }

            var summarizedConcepts = contentSummarizer.GetConcepts(analyzedDocument, arguments);
            if (summarizedConcepts == null)
            {
                throw new InvalidOperationException(string.Format("{0}.GetConcepts must not return null", contentSummarizer.GetType().FullName));
            }

            var summarizedSentences = contentSummarizer.GetSentences(analyzedDocument, arguments);
            if (summarizedSentences == null)
            {
                throw new InvalidOperationException(string.Format("{0}.GetSentences must not return null", contentSummarizer.GetType().FullName));
            }

            return new SummarizedDocument() { Concepts = summarizedConcepts, Sentences = summarizedSentences };
        }
    }
}