using System;
using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SummarizeThis(new FileContentProvider("TextualData\\AutomaticSummarization.txt"));
            SummarizeThis(new WikipediaContentProvider("Takhat"));
        }

        private static void SummarizeThis(IContentProvider contentProvider)
        {
            var summarizedDocument = Summarizer.Summarize(
                contentProvider,
                new SummarizerArguments {Language = "en", MaxSummarySentences = 5});

            Console.WriteLine("Summarizing content from " + contentProvider.GetType().FullName);
            Console.WriteLine(" ===== Concepts =============================== ");
            summarizedDocument.Concepts.ForEach(c => Console.WriteLine($"\t{c}"));
            Console.WriteLine(" ===== Summary =============================== ");
            summarizedDocument.Sentences.ForEach(s => Console.WriteLine($"{s}"));
            Console.ReadKey();
        }
    }
}