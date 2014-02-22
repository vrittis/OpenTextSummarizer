using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class Stemmer
    {

        internal static Word StemWord(string word, Dictionary rules)
        {
            word = word.ToLower();
            Word newword = new Word();
            newword.TermFrequency = 1;
            newword.Value = StemFormat(word, rules);
            newword.Stem = StemStrip(word, rules);
            return newword;
        }

        internal static string StemStrip(string word, Dictionary rules)
        {
            string originalWord = word;
            word = StemFormat(word, rules);
            word = ReplaceWord(word, rules.ManualReplacementRules);
            word = StripPrefix(word, rules.PrefixRules);
            word = StripSuffix(word, rules.SuffixRules);
            word = ReplaceWord(word, rules.SynonymRules);
            if (word.Length <= 2) word = StemFormat(originalWord, rules);
            return word;

        }

        internal static string StemFormat(string word, Dictionary rules)
        {
            word = StripPrefix(word, rules.Step1PrefixRules);
            word = StripSuffix(word, rules.Step1SuffixRules);
            return word;
        }

        private static string StripSuffix(string word, Dictionary<string, string> suffixRule)
        {
            //not simply using .Replace() in this method in case the 
            //rule.Key exists multiple times in the string.
            foreach (KeyValuePair<string, string> rule in suffixRule)
            {
                if (word.EndsWith(rule.Key))
                {
                    word = word.Substring(0, word.Length - rule.Key.Length) + rule.Value;
                }
            }

            return word;
        }

        private static string ReplaceWord(string word, Dictionary<string, string> replacementRule)
        {
            foreach (KeyValuePair<string, string> rule in replacementRule)
            {
                if (word == rule.Key)
                {
                    return rule.Value;
                }
            }

            return word;
        }

        private static string StripPrefix(string word, Dictionary<string, string> prefixRule)
        {
            //not simply using .Replace() in this method in case the 
            //rule.Key exists multiple times in the string.
            foreach (KeyValuePair<string, string> rule in prefixRule)
            {
                if (word.StartsWith(rule.Key))
                {
                    word = rule.Value + word.Substring(rule.Key.Length);
                }
            }
            return word;

        }

    }
}
