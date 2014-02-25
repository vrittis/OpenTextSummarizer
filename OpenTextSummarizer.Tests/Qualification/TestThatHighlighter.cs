using NUnit.Framework;
using System.Linq;
using OTS = OpenTextSummarizer;

namespace OpenTextSummarizer.Tests.Highlighter
{
    internal abstract class TestThatHighlighter
    {
        internal OTS.Highlighter Target { get; set; }

        internal OTS.Article TargetArticle { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new OTS.Highlighter();
            TargetArticle = new OTS.Article(null, null)
            {
                Sentences = Enumerable.Range(0, 10).Select(i => new Sentence()
                {
                    Score = i,
                    OriginalSentence = i.ToString(),
                    Words = Enumerable.Range(0, 10).Select(c => new OTS.Word()).ToList()
                }).ToList()
            };
        }

        public class Highlight : TestThatHighlighter
        {
            [Test, Category("Qualification")]
            public void highlight_returns_no_selection_if_no_selection_value_is_set()
            {
                Target.Highlight(TargetArticle, new OTS.SummarizerArguments() { DisplayLines = 0, DisplayPercent = 0 });
                Assert.AreEqual(0, TargetArticle.Sentences.Where(s => s.Selected).Count());
            }

            [Test, Category("Qualification")]
            public void if_percentage_is_not_set_use_linecount()
            {
                Target.Highlight(TargetArticle, new OTS.SummarizerArguments() { DisplayLines = 3, DisplayPercent = 0 });
                Assert.AreEqual(3, TargetArticle.Sentences.Where(s => s.Selected).Count());
            }

            [Test, Category("Qualification")]
            public void if_percentage_is_set_use_percentage()
            {
                Target.Highlight(TargetArticle, new OTS.SummarizerArguments() { DisplayLines = 3, DisplayPercent = 20 });
                Assert.AreEqual(2, TargetArticle.Sentences.Where(s => s.Selected).Count());
            }
        }

        public class SelectSentencesByPercent : TestThatHighlighter
        {
            [Test, Category("Qualification")]
            public void marks_the_correct_number_of_lines_as_selected()
            {
                Target.SelectSentencesByPercent(TargetArticle, 20);
                Assert.AreEqual(2, TargetArticle.Sentences.Where(s => s.Selected).Count());
            }

            [Test, Category("Qualification")]
            public void selects_all_lines_for_percentage_over_100()
            {
                Target.SelectSentencesByPercent(TargetArticle, 1000);
                Assert.AreEqual(10, TargetArticle.Sentences.Where(s => s.Selected).Count());
            }

            [Test, Category("Qualification")]
            public void selects_1_percent_of_the_lines_for_percentage_under_1()
            {
                Target.SelectSentencesByPercent(TargetArticle, 0);
                Assert.AreEqual(1, TargetArticle.Sentences.Where(s => s.Selected).Count());
            }
        }

        public class SelectNumberOfSentences : TestThatHighlighter
        {
            [Test, Category("Qualification")]
            public void marks_the_correct_number_of_lines_as_selected()
            {
                Target.SelectNumberOfSentences(TargetArticle, 3);
                var SelectedSentences = TargetArticle.Sentences.Where(s => s.Selected);

                Assert.AreEqual(3, SelectedSentences.Count());
            }

            [Test, Category("Qualification")]
            public void marks_the_top_scored_sentences_as_selected()
            {
                Target.SelectNumberOfSentences(TargetArticle, 3);
                var SelectedSentencesScore = TargetArticle.Sentences.Where(s => s.Selected).Select(s => s.Score);

                Assert.IsTrue(SelectedSentencesScore.All(d => new double[] { 9, 8, 7 }.Contains(d)));
            }

            [Test, Category("Qualification")]
            public void ignores_sentences_where_original_sentence_is_null()
            {
                TargetArticle.Sentences.ForEach(s => s.OriginalSentence = null);
                Target.SelectNumberOfSentences(TargetArticle, 3);
                Assert.AreEqual(0, TargetArticle.Sentences.Where(s => s.Selected).Count());
            }
        }
    }
}