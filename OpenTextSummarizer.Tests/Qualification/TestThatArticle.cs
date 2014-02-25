using NUnit.Framework;
using System.Linq;
using OTS = OpenTextSummarizer;

namespace OpenTextSummarizer.Tests.Article
{
    internal abstract class TestThatArticle
    {
        internal OTS.Article Target { get; set; }

        [SetUp]
        public void before_each_test()
        {
            Target = new OTS.Article(Factories.DictionaryFactory.Build(), new OTS.Stemmer());
        }

        public class ParseText : TestThatArticle
        {
            [Test]
            public void words_are_split_on_space_and_carriage_return()
            {
                Target.ParseText("word1 word2 word3. word4\rword5 \rword6");
                Assert.AreEqual(6, Target.WordCounts.Count());
            }

            [Test]
            public void words_frequency_match_their_appearances()
            {
                Target.ParseText("word otherword word otherword");
                Assert.IsTrue(Target.WordCounts.All(w => w.TermFrequency == 2));
            }
        }
    }
}