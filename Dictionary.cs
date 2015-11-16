using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Xml.Linq;

namespace OpenTextSummarizer
{
    internal class Dictionary
    {
        public List<string> UnimportantWords { get; set; }

        public List<string> LinebreakRules { get; set; }

        public List<string> NotALinebreakRules { get; set; }

        public List<string> DepreciateValueRule { get; set; }

        public List<string> TermFreqMultiplierRule { get; set; }

        //the replacement rules are stored as KeyValuePair<string,string>s
        //the Key is the search term. the Value is the replacement term
        public Dictionary<string, string> Step1PrefixRules { get; set; }

        public Dictionary<string, string> Step1SuffixRules { get; set; }

        public Dictionary<string, string> ManualReplacementRules { get; set; }

        public Dictionary<string, string> PrefixRules { get; set; }

        public Dictionary<string, string> SuffixRules { get; set; }

        public Dictionary<string, string> SynonymRules { get; set; }

        public string Language { get; set; }

        [FileIOPermission(SecurityAction.Demand, Read = "$AppDir$\\dics")]
        public static Dictionary LoadFromFile(string dictionaryLanguage)
        {
            var dictionaryFile = string.Format(@"{1}\dics\{0}.xml", dictionaryLanguage,
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)?.Substring(6));
            if (!File.Exists(dictionaryFile))
            {
                throw new FileNotFoundException("Could Not Load Dictionary: " + dictionaryFile);
            }
            var dict = new Dictionary();
            var doc = XElement.Load(dictionaryFile);
            dict.Step1PrefixRules = LoadKeyValueRule(doc, "stemmer", "step1_pre");
            dict.Step1SuffixRules = LoadKeyValueRule(doc, "stemmer", "step1_post");
            dict.ManualReplacementRules = LoadKeyValueRule(doc, "stemmer", "manual");
            dict.PrefixRules = LoadKeyValueRule(doc, "stemmer", "pre");
            dict.SuffixRules = LoadKeyValueRule(doc, "stemmer", "post");
            dict.SynonymRules = LoadKeyValueRule(doc, "stemmer", "synonyms");
            dict.LinebreakRules = LoadValueOnlyRule(doc, "parser", "linebreak");
            dict.NotALinebreakRules = LoadValueOnlyRule(doc, "parser", "linedontbreak");
            dict.DepreciateValueRule = LoadValueOnlyRule(doc, "grader-syn", "depreciate");
            dict.TermFreqMultiplierRule = LoadValueOnlySection(doc, "grader-tf");

            dict.UnimportantWords = new List<string>();
            var unimpwords = LoadValueOnlySection(doc, "grader-tc");
            foreach (var unimpword in unimpwords)
            {
                dict.UnimportantWords.Add(unimpword);
            }
            return dict;
        }

        private static List<string> LoadValueOnlySection(XElement doc, string section)
        {
            var step1Pre = doc.Elements(section);
            return step1Pre.Elements().Select(x => x.Value).ToList();
        }

        private static List<string> LoadValueOnlyRule(XElement doc, string section, string container)
        {
            var step1Pre = doc.Elements(section).Elements(container);
            return step1Pre.Elements().Select(x => x.Value).ToList();
        }

        private static Dictionary<string, string> LoadKeyValueRule(XElement doc, string section, string container)
        {
            var dictionary = new Dictionary<string, string>();
            var step1Pre = doc.Elements(section).Elements(container);
            foreach (
                var keyvalue in
                    step1Pre.Elements()
                        .Select(x => x.Value)
                        .Select(rule => rule.Split('|'))
                        .Where(keyvalue => !dictionary.ContainsKey(keyvalue[0])))
            {
                dictionary.Add(keyvalue[0], keyvalue[1]);
            }
            return dictionary;
        }
    }
}