using NUnit.Framework;
using System.Collections.Generic;
using OTS = OpenTextSummarizer;

namespace OpenTextSummarizer.Tests.Stemmer
{
    public abstract class TestThatStemmer
    {
        internal OTS.Stemmer Target { get; set; }

        internal OTS.Dictionary TargetDic { get; set; }

        internal string Word { get; set; }

        [SetUp]
        public void before_any_test()
        {
            Target = new OTS.Stemmer();
            TargetDic = Factories.DictionaryFactory.Build();
            Word = "overqualification";
        }

        public class ReplaceWord : TestThatStemmer
        {
            [Test, Category("Qualification")]
            public void doesnt_replace_part_of_a_word()
            {
                TargetDic.ManualReplacementRules.Add(Word, "word");
                Assert.AreNotEqual("word", Target.ReplaceWord("Test" + Word, TargetDic.ManualReplacementRules));
                Assert.AreNotEqual("word", Target.ReplaceWord(Word + "Test", TargetDic.ManualReplacementRules));
                Assert.AreNotEqual("word", Target.ReplaceWord("Test" + Word + "Test", TargetDic.ManualReplacementRules));
            }

            [Test, Category("Qualification")]
            public void doesnt_replace_words_multiple_times_when_rule_order_allows_it()
            {
                TargetDic.ManualReplacementRules.Add(Word, "word");
                TargetDic.ManualReplacementRules.Add("word", "otherword");
                Assert.AreEqual("word", Target.ReplaceWord(Word, TargetDic.ManualReplacementRules));
            }

            [Test, Category("Qualification")]
            public void doesnt_replace_words_with_non_matching_replace_rules()
            {
                TargetDic.ManualReplacementRules.Add("word", "otherword");
                Assert.AreEqual(Word, Target.ReplaceWord(Word, TargetDic.ManualReplacementRules));
            }

            [Test, Category("Qualification")]
            public void doesnt_replace_words_without_replace_rules()
            {
                Assert.AreEqual(Word, Target.ReplaceWord(Word, TargetDic.ManualReplacementRules));
            }

            [Test, Category("Qualification")]
            public void replaces_words_single_time_when_rule_order_doesnt_allow_it()
            {
                TargetDic.ManualReplacementRules.Add("word", "otherword");
                TargetDic.ManualReplacementRules.Add(Word, "word");
                Assert.AreEqual("word", Target.ReplaceWord(Word, TargetDic.ManualReplacementRules));
            }

            [Test, Category("Qualification")]
            public void replaces_words_with_matching_replace_rules()
            {
                TargetDic.ManualReplacementRules.Add(Word, "word");
                Assert.AreEqual("word", Target.ReplaceWord(Word, TargetDic.ManualReplacementRules));
            }
        }

        public class StemFormat : TestThatStemmer
        {
            [Test, Category("Qualification")]
            public void doesnt_lowercases_word()
            {
                Assert.AreNotEqual(Word, Target.ReplaceWord(Word.ToUpper(), TargetDic.ManualReplacementRules));
            }
        }

        public class StemStrip : TestThatStemmer
        {
            [Test, Category("Qualification")]
            public void doesnt_lowercases_word()
            {
                Assert.AreNotEqual(Word, Target.StemStrip(Word.ToUpper(), TargetDic));
            }

            [Test, Category("Qualification")]
            public void if_formatted_word_ends_lower_than_2_caracters_original_word_is_used_after_formatting_without_lowercasing()
            {
                TargetDic.ManualReplacementRules.Add(Word, "NA");
                TargetDic.Step1PrefixRules.Add("OVER", "NA");
                Assert.AreEqual("NAQUALIFICATION", Target.StemStrip(Word.ToUpper(), TargetDic));
            }
        }

        public class StemWord : TestThatStemmer
        {
            [Test, Category("Qualification")]
            public void keeps_word_as_stem_when_no_rules_apply()
            {
                Assert.AreEqual(Word, Target.StemWord(Word, TargetDic).Stem);
            }

            [Test, Category("Qualification")]
            public void lowercases_word_value()
            {
                Assert.AreEqual(Word, Target.StemWord(Word.ToUpper(), TargetDic).Value);
            }
        }

        public class StripPrefix : TestThatStemmer
        {
            [Test, Category("Qualification")]
            public void acknowledges_casing_in_prefix_rules()
            {
                TargetDic.PrefixRules.Add("Over", "CHANGE");
                Assert.AreNotEqual("CHANGEqualification", Target.StripPrefix(Word, TargetDic.PrefixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_once_prefix_rules_matching_word_are_in_the_right_order()
            {
                TargetDic.PrefixRules.Add("quali", "CHANGE");
                TargetDic.PrefixRules.Add("over", "CHANGE");
                Assert.AreEqual("CHANGEqualification", Target.StripPrefix(Word, TargetDic.PrefixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_once_prefix_rules_matching_word_are_in_the_right_order_and_rules_rewrite_word()
            {
                TargetDic.PrefixRules.Add("over", "CHANGE");
                TargetDic.PrefixRules.Add("quali", "CHANGE");
                Assert.AreEqual("CHANGEqualification", Target.StripPrefix(Word, TargetDic.PrefixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_twice_prefix_rules_matching_word_are_in_the_right_order_and_rules_dont_rewrite_word()
            {
                TargetDic.PrefixRules.Add("over", "");
                TargetDic.PrefixRules.Add("quali", "");
                Assert.AreEqual("fication", Target.StripPrefix(Word, TargetDic.PrefixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_when_prefix_rules_match_word()
            {
                TargetDic.PrefixRules.Add("over", "CHANGE");
                Assert.AreEqual("CHANGEqualification", Target.StripPrefix(Word, TargetDic.PrefixRules));
            }

            [Test, Category("Qualification")]
            public void doesnt_change_word_when_no_prefix_rules_exist()
            {
                Assert.AreEqual(Word, Target.StripPrefix(Word, TargetDic.PrefixRules));
            }

            [Test, Category("Qualification")]
            public void doesnt_match_inner_part_of_word()
            {
                TargetDic.PrefixRules.Add("over", "CHANGE");
                Assert.AreEqual("CHANGEoverqualification", Target.StripPrefix("over" + Word, TargetDic.PrefixRules));
            }
        }

        public class StripSuffix : TestThatStemmer
        {
            [Test, Category("Qualification")]
            public void acknowledges_casing_in_suffix_rules()
            {
                TargetDic.SuffixRules.Add("Ication", "y");
                Assert.AreNotEqual("overqualify", Target.StripSuffix(Word, TargetDic.SuffixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_once_suffix_rules_matching_word_are_in_the_right_order()
            {
                TargetDic.SuffixRules.Add("icat", "y");
                TargetDic.SuffixRules.Add("ion", "e");
                Assert.AreEqual("overqualificate", Target.StripSuffix(Word, TargetDic.SuffixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_once_suffix_rules_matching_word_are_in_the_right_order_and_rules_rewrite_word()
            {
                TargetDic.SuffixRules.Add("ation", "ate");
                TargetDic.SuffixRules.Add("ic", "y");
                Assert.AreEqual("overqualificate", Target.StripSuffix(Word, TargetDic.SuffixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_twice_suffix_rules_matching_word_are_in_the_right_order_and_rules_dont_rewrite_word()
            {
                TargetDic.SuffixRules.Add("ation", "");
                TargetDic.SuffixRules.Add("ic", "y");
                Assert.AreEqual("overqualify", Target.StripSuffix(Word, TargetDic.SuffixRules));
            }

            [Test, Category("Qualification")]
            public void changes_word_when_suffix_rules_match_word()
            {
                TargetDic.SuffixRules.Add("ication", "y");
                Assert.AreEqual("overqualify", Target.StripSuffix(Word, TargetDic.SuffixRules));
            }

            [Test, Category("Qualification")]
            public void doesnt_change_word_when_no_suffix_rules_exist()
            {
                Assert.AreEqual(Word, Target.StripSuffix(Word, TargetDic.SuffixRules));
            }

            [Test, Category("Qualification")]
            public void doesnt_match_inner_part_of_word()
            {
                TargetDic.SuffixRules.Add("ication", "y");
                Assert.AreEqual("overqualificationy", Target.StripSuffix(Word + "ication", TargetDic.SuffixRules));
            }
        }
    }
}