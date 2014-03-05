using OpenTextSummarizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTextSummarizer
{
    internal class TextUnitBuilder : ITextUnitBuilder
    {
        internal Dictionary m_Rules { get; set; }

        public TextUnitBuilder(Dictionary Rules)
        {
            m_Rules = Rules;
        }

        public TextUnit Build(string word)
        {
            var builtTextUnit = new TextUnit();
            builtTextUnit.RawValue = word;
            builtTextUnit.FormattedValue = Format(builtTextUnit.RawValue);
            builtTextUnit.Stem = Stem(builtTextUnit.FormattedValue);
            if (builtTextUnit.Stem.Length <= 2)
            {
                builtTextUnit.Stem = builtTextUnit.FormattedValue;
            }
            return builtTextUnit;
        }

        internal string Stem(string word)
        {
            word = ReplaceWord(word, m_Rules.ManualReplacementRules);
            word = StripPrefix(word, m_Rules.PrefixRules);
            word = StripSuffix(word, m_Rules.SuffixRules);
            word = ReplaceWord(word, m_Rules.SynonymRules);

            return word;
        }

        internal string Format(string word)
        {
            word = StripPrefix(word, m_Rules.Step1PrefixRules);
            word = StripSuffix(word, m_Rules.Step1SuffixRules);
            return word;
        }

        public string StripSuffix(string word, Dictionary<string, string> suffixRules)
        {
            var suffixRule = suffixRules.FirstOrDefault(kvp => word.EndsWith(kvp.Key));
            if (!suffixRule.Equals(default(KeyValuePair<string, string>)))
            {
                //not simply using .Replace() in this method in case the
                //rule.Key exists multiple times in the string.
                word = word.Substring(0, word.Length - suffixRule.Key.Length) + suffixRule.Value;
            }
            return word;
        }

        internal string ReplaceWord(string word, Dictionary<string, string> replacementRules)
        {
            var replacementRule = replacementRules.FirstOrDefault(kvp => kvp.Key == word);
            if (!replacementRule.Equals(default(KeyValuePair<string, string>)))
            {
                word = replacementRule.Value;
            }
            return word;
        }

        internal string StripPrefix(string word, Dictionary<string, string> prefixRules)
        {
            var prefixRule = prefixRules.FirstOrDefault(kvp => word.StartsWith(kvp.Key));
            if (!prefixRule.Equals(default(KeyValuePair<string, string>)))
            {
                //not simply using .Replace() in this method in case the
                //rule.Key exists multiple times in the string.
                word = prefixRule.Value + word.Substring(prefixRule.Key.Length);
            }
            return word;
        }
    }
}