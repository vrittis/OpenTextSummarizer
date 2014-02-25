using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTS = OpenTextSummarizer;

namespace OpenTextSummarizer.Tests.Word
{
    [TestFixture]
    public abstract class TestThatWord
    {
        internal OTS.Word TargetNull = null;
        internal OTS.Word TargetWord1 = null;
        internal OTS.Word TargetWord2 = null;
        [SetUp]
        public void BeforeEachTest()
        {
            TargetNull = new OTS.Word();
            TargetWord1 = new OTS.Word("Word1");
            TargetWord2 = new OTS.Word("Word2");
        }

        public class Constructor: TestThatWord
        {
            [Test, Category("Qualification")]
            public void sets_value_to_null_with_the_empty_constructor()
            {
                Assert.IsNull(TargetNull.Value);
            }

            [Test, Category("Qualification")]
            public void sets_value_to_passed_argument()
            {
                Assert.AreEqual("word", new OTS.Word("word").Value);
            }
        }

        public class MethodEquals: TestThatWord
        {
            [Test, Category("Qualification"), ExpectedException(typeof(NullReferenceException))]
            public void throws_when_comparing_empty_built_word_with_another_word()
            {
                TargetNull.Equals(new OTS.Word());
            }

            [Test, Category("Qualification")]
            public void returns_false_when_comparing_to_null_word()
            {
                Assert.AreEqual(false, TargetWord1.Equals(TargetNull));
            }

            [Test, Category("Qualification")]
            public void returns_false_when_comparing_to_different_word()
            {
                Assert.AreEqual(false, TargetWord1.Equals(TargetWord2));
            }

            [Test, Category("Qualification")]
            public void returns_true_when_comparing_to_same_word()
            {
                Assert.AreEqual(true, TargetWord1.Equals(TargetWord1));
            }

            [Test, Category("Qualification")]
            public void returns_true_when_comparing_to_same_word_built_apart()
            {
                Assert.AreEqual(true, TargetWord1.Equals(new OTS.Word("Word1")));
            }

            [Test, Category("Qualification")]
            public void returns_false_when_comparing_to_object_of_a_different_type()
            {
                Assert.AreNotEqual(true, TargetWord1.Equals(new object()));
            }
        }

        public class MethodGetHashCode: TestThatWord
        {
            [Test, Category("Qualification")]
            public void returns_the_same_hashcode_as_its_value()
            {
                Assert.AreEqual(TargetWord1.Value.GetHashCode(), TargetWord1.GetHashCode());
            }
        }

        public class MethodToString : TestThatWord
        {
            [Test, Category("Qualification")]
            public void returns_the_value()
            {
                Assert.AreEqual(TargetWord1.Value, TargetWord1.ToString());
            }
        }
    }
}
