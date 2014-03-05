using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class SummarizingEngine
    {
        public ParsedDocument ParseContent(IContentProvider contentProvider, IContentParser contentParser)
        {
            var resultingParsedDocument = new ParsedDocument();
            resultingParsedDocument.Sentences = contentParser.SplitContentIntoSentences(contentProvider.Content);
            foreach (var workingSentence in resultingParsedDocument.Sentences)
            {
                workingSentence.TextUnits = contentParser.SplitSentenceIntoTextUnits(workingSentence.OriginalSentence);
            }
            return resultingParsedDocument;
        }

        public AnalyzedDocument AnalyzeParsedContent(ParsedDocument parsedDocument, IContentAnalyzer contentAnalyzer)
        {
            var importantTextUnits = contentAnalyzer.GetImportantTextUnits(parsedDocument.Sentences);
            var scoredSentences = contentAnalyzer.ScoreSentences(parsedDocument.Sentences, importantTextUnits);

            return new AnalyzedDocument() { ScoredTextUnits = importantTextUnits, ScoredSentences = scoredSentences };
        }

        public SummarizedDocument SummarizeAnalysedContent(AnalyzedDocument analyzedDocument, IContentSummarizer contentSummarizer, SummarizerArguments arguments)
        {
            return new SummarizedDocument() { Concepts = contentSummarizer.GetConcepts(analyzedDocument, arguments), Sentences = contentSummarizer.GetSentences(analyzedDocument, arguments) };
        }
    }
}