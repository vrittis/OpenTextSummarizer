using System.Collections.Generic;
using System.Linq;
using OpenTextSummarizer.Interfaces;

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
            builtTextUnit.RawValue = word.ToLower();
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
            //not simply using .Replace() in this method in case the 
            //rule.Key exists multiple times in the string.
            foreach (KeyValuePair<string, string> rule in suffixRules)
            {
                if (word.EndsWith(rule.Key))
                {
                    word = word.Substring(0, word.Length - rule.Key.Length) + rule.Value;
                }
            }

            return word;
        }

        internal string ReplaceWord(string word, Dictionary<string, string> replacementRules)
        {
            foreach (KeyValuePair<string, string> rule in replacementRules)
            {
                if (word == rule.Key)
                {
                    return rule.Value;
                }
            }

            return word;
        }

        internal string StripPrefix(string word, Dictionary<string, string> prefixRules)
        {
            //not simply using .Replace() in this method in case the 
            //rule.Key exists multiple times in the string.
            foreach (KeyValuePair<string, string> rule in prefixRules)
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