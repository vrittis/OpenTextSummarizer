using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer
{
    public class Summarizer
    {
        public static SummarizedDocument Summarize(IContentProvider contentProvider, ISummarizerArguments arguments)
        {
            if (contentProvider == null || arguments == null)
            {
                return new SummarizedDocument();
            }

            var engine = new SummarizingEngine();

            var parsedDocument = engine.ParseContent(contentProvider, arguments.ContentParser());
            var analyzedDocument = engine.AnalyzeParsedContent(parsedDocument, arguments.ContentAnalyzer());
            var summaryAnalysisDocument = engine.SummarizeAnalysedContent(analyzedDocument, arguments.ContentSummarizer(), arguments);

            return summaryAnalysisDocument;
        }
    }
}