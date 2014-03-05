using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTextSummarizer.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SummarizeThis(new OpenTextSummarizer.FileContentProvider("TextualData\\AutomaticSummarization.txt"));
            SummarizeThis(new WikipediaContentProvider("Takhat"));
        }

        private static void SummarizeThis(IContentProvider contentProvider)
        {
            var summarizedDocument = OpenTextSummarizer.Summarizer.Summarize(
                contentProvider,
                new SummarizerArguments() { Language = "en", MaxSummarySentences = 5 });

            Console.WriteLine("Summarizing content from " + contentProvider.GetType().FullName);
            Console.WriteLine(" ===== Concepts =============================== ");
            summarizedDocument.Concepts.ForEach(c => Console.WriteLine(string.Format("\t{0}", c)));
            Console.WriteLine(" ===== Summary =============================== ");
            summarizedDocument.Sentences.ForEach(s => Console.WriteLine(string.Format("{0}", s)));
            Console.ReadKey();
        }
    }
}