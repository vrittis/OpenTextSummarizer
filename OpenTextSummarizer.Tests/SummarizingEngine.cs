using NSubstitute;
using NUnit.Framework;
using OpenTextSummarizer.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenTextSummarizer.Tests
{
    [TestFixture]
    public abstract class SummarizingEngine
    {
        internal OpenTextSummarizer.SummarizingEngine Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new OpenTextSummarizer.SummarizingEngine();
        }

        public class ParseContent : SummarizingEngine
        {
            internal IContentProvider TargetContentProvider { get; set; }

            internal IContentParser TargetContentParser { get; set; }

            internal const string Content = "this is content";
            internal const string Sentence1 = "is sentence one";
            internal const string Sentence2 = "is sentence two";

            [SetUp]
            public void before_each_test_setup()
            {
                TargetContentProvider = Substitute.For<IContentProvider>();
                TargetContentProvider.Content.Returns(Content);
                TargetContentParser = Substitute.For<IContentParser>();
                TargetContentParser.SplitContentIntoSentences(Arg.Any<string>()).Returns(
                    new List<Sentence>
                    {
                        new Sentence { OriginalSentence = Sentence1 } ,
                        new Sentence { OriginalSentence = Sentence2 }
                    }
                );
                TargetContentParser.SplitSentenceIntoTextUnits(Arg.Any<string>()).Returns(
                    new List<TextUnit>
                    {
                        new TextUnit { RawValue = Content}
                    }
                );
            }

            public static IEnumerable NullArgumentsSource
            {
                get
                {
                    yield return new TestCaseData(null, Substitute.For<IContentParser>());
                    yield return new TestCaseData(Substitute.For<IContentProvider>(), null);
                    yield return new TestCaseData(null, null);
                }
            }

            [Test, TestCaseSource("NullArgumentsSource")]
            public void throws_if_null_arguments_are_passed(IContentProvider contentProvider, IContentParser contentParser)
            {
                Assert.That(() => Target.ParseContent(contentProvider, contentParser), Throws.TypeOf<ArgumentNullException>());
            }

            [Test]
            public void calls_IContentProvider_Content_property()
            {
                Target.ParseContent(TargetContentProvider, TargetContentParser);
                string result = TargetContentProvider.Received(1).Content;
            }

            [Test]
            public void calls_IContentParser_SplitContentIntoSentences_with_the_content_of_the_IContentProvider()
            {
                Target.ParseContent(TargetContentProvider, TargetContentParser);
                TargetContentParser.Received(1).SplitContentIntoSentences(Content);
            }

            [Test]
            public void calls_IContentParser_SplitSentenceIntoTextUnits_with_each_sentence_content()
            {
                Target.ParseContent(TargetContentProvider, TargetContentParser);
                TargetContentParser.Received(1).SplitSentenceIntoTextUnits(Sentence1);
                TargetContentParser.Received(1).SplitSentenceIntoTextUnits(Sentence2);
            }

            [Test]
            public void returns_ParsedDocument_containing_correct_number_of_sentences()
            {
                var parsedDocument = Target.ParseContent(TargetContentProvider, TargetContentParser);
                Assert.AreEqual(2, parsedDocument.Sentences.Count);
            }

            [Test]
            public void returns_ParsedDocument_containing_sentences_with_returned_text_units()
            {
                var parsedDocument = Target.ParseContent(TargetContentProvider, TargetContentParser);
                Assert.IsTrue(parsedDocument.Sentences.All(s => s.TextUnits.Count == 1));
            }
        }

        public class AnalyzeParsedContent : SummarizingEngine
        {
            internal ParsedDocument TargetParsedDocument { get; set; }

            internal IContentAnalyzer TargetContentAnalyzer { get; set; }

            internal SummarizationInformationContainer TargetContainer { get; set; }

            [SetUp]
            public void before_each_test_setup()
            {
                TargetContainer = new SummarizationInformationContainer();

                TargetParsedDocument = new ParsedDocument();
                TargetParsedDocument.Sentences = new List<Sentence>()
                {
                   TargetContainer.Sentence
                };

                TargetContentAnalyzer = Substitute.For<IContentAnalyzer>();
                TargetContentAnalyzer.GetImportantTextUnits(Arg.Any<List<Sentence>>()).Returns(TargetContainer.ScoredTextUnits);
                TargetContentAnalyzer.ScoreSentences(Arg.Any<List<Sentence>>(), Arg.Any<List<TextUnitScore>>()).Returns(TargetContainer.ScoredSentences);
            }

            public static IEnumerable NullArgumentsSource
            {
                get
                {
                    yield return new TestCaseData(null, Substitute.For<IContentAnalyzer>());
                    yield return new TestCaseData(new ParsedDocument(), null);
                    yield return new TestCaseData(null, null);
                }
            }

            [Test, TestCaseSource("NullArgumentsSource")]
            public void throws_if_null_arguments_are_passed(ParsedDocument parsedDocument, IContentAnalyzer contentAnalyzer)
            {
                Assert.That(() => Target.AnalyzeParsedContent(parsedDocument, contentAnalyzer), Throws.TypeOf<ArgumentNullException>());
            }

            [Test]
            public void throws_if_IContentAnalyzer_GetImportantTextUnits_returns_null()
            {
                TargetContentAnalyzer.GetImportantTextUnits(Arg.Any<List<Sentence>>()).Returns((List<TextUnitScore>)null);
                Assert.That(() => Target.AnalyzeParsedContent(TargetParsedDocument, TargetContentAnalyzer), Throws.TypeOf<InvalidOperationException>());
            }

            [Test]
            public void throws_if_IContentAnalyzer_ScoreSentences_returns_null()
            {
                TargetContentAnalyzer.ScoreSentences(Arg.Any<List<Sentence>>(), Arg.Any<List<TextUnitScore>>()).Returns((List<SentenceScore>)null);
                Assert.That(() => Target.AnalyzeParsedContent(TargetParsedDocument, TargetContentAnalyzer), Throws.TypeOf<InvalidOperationException>());
            }

            [Test]
            public void calls_IContentAnalyzer_GetImportantTextUnits_with_parsed_document_sentences()
            {
                Target.AnalyzeParsedContent(TargetParsedDocument, TargetContentAnalyzer);
                TargetContentAnalyzer.Received(1).GetImportantTextUnits(TargetParsedDocument.Sentences);
            }

            [Test]
            public void calls_IContentAnalyzer_ScoreSentences_with_parsed_document_sentences_and_returning_scored_text_units()
            {
                Target.AnalyzeParsedContent(TargetParsedDocument, TargetContentAnalyzer);
                TargetContentAnalyzer.Received(1).ScoreSentences(TargetParsedDocument.Sentences, TargetContainer.ScoredTextUnits);
            }

            [Test]
            public void orders_results_from_IContentAnalyzer_into_AnalyzedDocument()
            {
                var result = Target.AnalyzeParsedContent(TargetParsedDocument, TargetContentAnalyzer);
                var resultSentenceScoreOrder = result.ScoredSentences.Select(s => s.Score);
                var currentSentenceScoreOrder = TargetContainer.ScoredSentences.OrderByDescending(s => s.Score).Select(s => s.Score).ToList();
                Assert.IsTrue(resultSentenceScoreOrder.SequenceEqual(currentSentenceScoreOrder), "Scored sentences have not been ordered in the analyzed document");

                var resultTextUnitScoreOrder = result.ScoredTextUnits.Select(s => s.Score);
                var currentTextUnitScoreOrder = TargetContainer.ScoredTextUnits.OrderByDescending(s => s.Score).Select(s => s.Score).ToList();
                Assert.IsTrue(resultTextUnitScoreOrder.SequenceEqual(currentTextUnitScoreOrder), "Scored text units have not been ordered in the analyzed document");
            }
        }

        public class SummarizeAnalyzedContent : SummarizingEngine
        {
            public AnalyzedDocument TargetAnalyzedDocument { get; set; }

            public IContentSummarizer TargetContentSummarizer { get; set; }

            public ISummarizerArguments TargetSummarizerArguments { get; set; }

            internal SummarizationInformationContainer TargetContainer { get; set; }

            [SetUp]
            public void before_each_test_setup()
            {
                TargetContainer = new SummarizationInformationContainer();

                TargetAnalyzedDocument = new AnalyzedDocument
                {
                    ScoredSentences = TargetContainer.ScoredSentences,
                    ScoredTextUnits = TargetContainer.ScoredTextUnits
                };

                TargetContentSummarizer = Substitute.For<IContentSummarizer>();
                TargetContentSummarizer.GetConcepts(Arg.Any<AnalyzedDocument>(), Arg.Any<ISummarizerArguments>()).Returns(new List<string>() { "concept" });
                TargetContentSummarizer.GetSentences(Arg.Any<AnalyzedDocument>(), Arg.Any<ISummarizerArguments>()).Returns(new List<string>() { "sentence" });

                TargetSummarizerArguments = Substitute.For<ISummarizerArguments>();
            }

            public static IEnumerable NullArgumentsSource
            {
                get
                {
                    yield return new TestCaseData(null, null, Substitute.For<ISummarizerArguments>());
                    yield return new TestCaseData(null, Substitute.For<IContentSummarizer>(), null);
                    yield return new TestCaseData(new AnalyzedDocument(), null, null);

                    yield return new TestCaseData(new AnalyzedDocument(), Substitute.For<IContentSummarizer>(), null);
                    yield return new TestCaseData(new AnalyzedDocument(), null, Substitute.For<ISummarizerArguments>());
                    yield return new TestCaseData(null, Substitute.For<IContentSummarizer>(), Substitute.For<ISummarizerArguments>());

                    yield return new TestCaseData(null, null, null);
                }
            }

            [Test, TestCaseSource("NullArgumentsSource")]
            public void throws_if_null_arguments_are_passed(AnalyzedDocument analyzedDocument, IContentSummarizer contentSummarizer, ISummarizerArguments summarizerArguments)
            {
                Assert.That(() => Target.SummarizeAnalysedContent(analyzedDocument, contentSummarizer, summarizerArguments), Throws.TypeOf<ArgumentNullException>());
            }

            [Test]
            public void calls_IContentSummarizer_GetConcepts_with_arguments_from_the_call()
            {
                Target.SummarizeAnalysedContent(TargetAnalyzedDocument, TargetContentSummarizer, TargetSummarizerArguments);
                TargetContentSummarizer.Received(1).GetConcepts(TargetAnalyzedDocument, TargetSummarizerArguments);
            }

            [Test]
            public void calls_IContentSummarizer_GetSentences_with_arguments_from_the_call()
            {
                Target.SummarizeAnalysedContent(TargetAnalyzedDocument, TargetContentSummarizer, TargetSummarizerArguments);
                TargetContentSummarizer.Received(1).GetSentences(TargetAnalyzedDocument, TargetSummarizerArguments);
            }

            [Test]
            public void throws_if_IContentSummarizer_GetConcepts_returns_null()
            {
                TargetContentSummarizer.GetConcepts(Arg.Any<AnalyzedDocument>(), Arg.Any<ISummarizerArguments>()).Returns((List<string>)null);
                Assert.That(() => Target.SummarizeAnalysedContent(TargetAnalyzedDocument, TargetContentSummarizer, TargetSummarizerArguments), Throws.TypeOf<InvalidOperationException>());
            }

            [Test]
            public void throws_if_IContentSummarizer_GetSentences_returns_null()
            {
                TargetContentSummarizer.GetSentences(Arg.Any<AnalyzedDocument>(), Arg.Any<ISummarizerArguments>()).Returns((List<string>)null);
                Assert.That(() => Target.SummarizeAnalysedContent(TargetAnalyzedDocument, TargetContentSummarizer, TargetSummarizerArguments), Throws.TypeOf<InvalidOperationException>());
            }

            [Test]
            public void checks_and_corrects_downward_range_on_arguments()
            {
                TargetSummarizerArguments.MaxConceptsInPercent.Returns(-10);
                TargetSummarizerArguments.MaxSummarySentences.Returns(-10);
                TargetSummarizerArguments.MaxSummarySizeInPercent.Returns(-10);
                Target.SummarizeAnalysedContent(TargetAnalyzedDocument, TargetContentSummarizer, TargetSummarizerArguments);
                Assert.AreEqual(0, TargetSummarizerArguments.MaxConceptsInPercent);
                Assert.AreEqual(0, TargetSummarizerArguments.MaxSummarySentences);
                Assert.AreEqual(0, TargetSummarizerArguments.MaxSummarySizeInPercent);
            }
        }

        public class SummarizationInformationContainer
        {
            public const string TextUnitText = "textunit";
            public const string SentenceText = "sentence";
            public TextUnit TextUnit;
            public List<TextUnitScore> ScoredTextUnits;
            public Sentence Sentence;
            public List<SentenceScore> ScoredSentences;

            public SummarizationInformationContainer()
            {
                TextUnit = new TextUnit()
                {
                    RawValue = TextUnitText,
                    FormattedValue = TextUnitText,
                    Stem = TextUnitText
                };

                ScoredTextUnits = new List<TextUnitScore>
                {
                    new TextUnitScore { Score = 15, ScoredTextUnit = TextUnit},
                    new TextUnitScore { Score = 30, ScoredTextUnit = TextUnit}
                };

                Sentence = new Sentence
                {
                    OriginalSentence = SentenceText,
                    OriginalSentenceIndex = 0,
                    TextUnits = new List<TextUnit>
                    {
                        TextUnit
                    }
                };

                ScoredSentences = new List<SentenceScore>
                {
                    new SentenceScore {Score = 10, ScoredSentence = Sentence},
                    new SentenceScore {Score = 20, ScoredSentence = Sentence}
                };
            }
        }
    }
}