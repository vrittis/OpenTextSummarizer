using NUnit.Framework;
using System.Linq;
using OTS = OpenTextSummarizer;

namespace OpenTextSummarizer.Tests.Summarizer
{
    public abstract class TestThatSummarizer
    {
        public class Summarize : TestThatSummarizer
        {
            [Test, Category("Qualification")]
            public void returns_null_when_args_are_null()
            {
                Assert.IsNull(OTS.Summarizer.Summarize(null));
            }

            [Test, Category("Qualification")]
            public void returns_no_concepts_when_args_have_no_datasource()
            {
                Assert.AreEqual(0, OTS.Summarizer.Summarize(new OTS.SummarizerArguments()).Concepts.Count);
            }

            [Test, Category("Qualification")]
            public void returns_no_sentences_when_args_have_no_datasource()
            {
                Assert.AreEqual(0, OTS.Summarizer.Summarize(new OTS.SummarizerArguments()).Sentences.Count);
            }
        }

        public class CreateSummarizedDocument : TestThatSummarizer
        {
            [Test, Category("Qualification")]
            public void copy_concepts_and_selected_sentences_to_summarized_document()
            {
                var Target = new OTS.Summarizer();
                var SummarizedDocument = Target.CreateSummarizedDocument(new OTS.Article(null, null)
                {
                    Sentences = Enumerable.Range(0, 3).Select(i => new Sentence()
                    {
                        Selected = true
                    }).ToList(),
                    Concepts = Enumerable.Range(0, 5).Select(i => i.ToString()).ToList()
                },
                new OTS.SummarizerArguments());

                Assert.AreEqual(3, SummarizedDocument.Sentences.Count());
                Assert.AreEqual(5, SummarizedDocument.Concepts.Count());
            }
        }
    }
}