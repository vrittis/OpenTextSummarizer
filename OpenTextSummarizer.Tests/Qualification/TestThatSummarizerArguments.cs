using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTS = OpenTextSummarizer;

namespace OpenTextSummarizer.Tests.SummarizerArguments
{
    public abstract class TestThatSummarizerArguments
    {
        [SetUp]
        public void BeforeEachTest()
        {
            Target = new OTS.SummarizerArguments();
        }

        internal OTS.SummarizerArguments Target = null;

        public class Constructor: TestThatSummarizerArguments
        {
            [Test, Category("Qualification")]
            public void sets_default_language_to_english()
            {
                Assert.AreEqual("en", Target.DictionaryLanguage);
            }

            [Test, Category("Qualification")]
            public void sets_default_line_percentage_to_10()
            {
                Assert.AreEqual(10, Target.DisplayPercent);
            }

            [Test, Category("Qualification")]
            public void sets_default_line_count_to_0()
            {
                Assert.AreEqual(0, Target.DisplayLines);
            }
        }
    }
}
