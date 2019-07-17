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
                throw new ArgumentNullException(nameof(contentProvider));
            }
            if (contentParser == null)
            {
                throw new ArgumentNullException(nameof(contentParser));
            }

            var resultingParsedDocument = new ParsedDocument
            {
                Sentences = contentParser
                    .SplitContentIntoSentences(contentProvider.Content)
                    .Where(sentence => !string.IsNullOrWhiteSpace(sentence.OriginalSentence))
                    .ToList()
            };
            
            foreach (Sentence workingSentence in resultingParsedDocument.Sentences)
            {
                workingSentence.TextUnits = contentParser
                    .SplitSentenceIntoTextUnits(workingSentence.OriginalSentence)
                    .Where(word => !string.IsNullOrWhiteSpace(word.RawValue))
                    .ToList();
            }

            return resultingParsedDocument;
        }

        /// <summary>
        /// Runs the content analysis part of the summarizing algorithm
        /// </summary>
        /// <param name="parsedDocument"></param>
        /// <param name="contentAnalyzer"></param>
        /// <returns></returns>
        public AnalyzedDocument AnalyzeParsedContent(ParsedDocument parsedDocument, IContentAnalyzer contentAnalyzer)
        {
            if (parsedDocument == null)
            {
                throw new ArgumentNullException(nameof(parsedDocument));
            }
            if (contentAnalyzer == null)
            {
                throw new ArgumentNullException(nameof(contentAnalyzer));
            }

            var importantTextUnits = contentAnalyzer.GetImportantTextUnits(parsedDocument.Sentences);
            if (importantTextUnits == null)
            {
                throw new InvalidOperationException($"{contentAnalyzer.GetType().FullName}.GetImportantTextUnits must not return null");
            }
            var scoredSentences = contentAnalyzer.ScoreSentences(parsedDocument.Sentences, importantTextUnits);
            if (scoredSentences == null)
            {
                throw new InvalidOperationException($"{contentAnalyzer.GetType().FullName}.ScoreSentences must not return null");
            }

            return new AnalyzedDocument
            {
                ScoredTextUnits = importantTextUnits.OrderByDescending(tus => tus.Score).ToList(),
                ScoredSentences = scoredSentences.OrderByDescending(ss => ss.Score).ToList()
            };
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
                throw new ArgumentNullException(nameof(analyzedDocument));
            }

            if (contentSummarizer == null)
            {
                throw new ArgumentNullException(nameof(contentSummarizer));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            // Range adjustment
            if (arguments.MaxConceptsInPercent < 0)
            {
                arguments.MaxConceptsInPercent = 0;
            }
            else if (arguments.MaxConceptsInPercent > 100)
            {
                arguments.MaxConceptsInPercent = 100;
            }

            if (arguments.MaxSummarySentences < 0)
            {
                arguments.MaxSummarySentences = 0;
            }

            if (arguments.MaxSummarySizeInPercent < 0)
            {
                arguments.MaxSummarySizeInPercent = 0;
            }
            else if (arguments.MaxSummarySizeInPercent > 100)
            {
                arguments.MaxSummarySizeInPercent = 100;
            }

            var summarizedConcepts = contentSummarizer.GetConcepts(analyzedDocument, arguments);
            if (summarizedConcepts == null)
            {
                throw new InvalidOperationException($"{contentSummarizer.GetType().FullName}.GetConcepts must not return null");
            }

            var summarizedSentences = contentSummarizer.GetSentences(analyzedDocument, arguments);
            if (summarizedSentences == null)
            {
                throw new InvalidOperationException($"{contentSummarizer.GetType().FullName}.GetSentences must not return null");
            }

            return new SummarizedDocument
            {
                Concepts = summarizedConcepts,
                Sentences = summarizedSentences
            };
        }
    }
}