using OpenTextSummarizer.Interfaces;
using System.Collections.Generic;

namespace OpenTextSummarizer
{
    internal class TextUnitBuilder : ITextUnitBuilder
    {
        internal LanguageData Rules { get; set; }

        public TextUnitBuilder(LanguageData rules)
        {
            Rules = rules;
        }

        public TextUnit Build(string word)
        {
            var builtTextUnit = new TextUnit { RawValue = word.ToLower() };
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
            word = ReplaceWord(word, Rules.ManualReplacementRules);
            word = StripPrefix(word, Rules.PrefixRules);
            word = StripSuffix(word, Rules.SuffixRules);
            word = ReplaceWord(word, Rules.SynonymRules);
            return word;
        }

        internal string Format(string word)
        {
            word = StripPrefix(word, Rules.Step1PrefixRules);
            word = StripSuffix(word, Rules.Step1SuffixRules);
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