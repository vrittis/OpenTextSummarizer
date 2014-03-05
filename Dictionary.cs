using System.Collections.Generic;
using System.IO;
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

        internal Dictionary()
        {
        }

        [FileIOPermission(SecurityAction.Demand, Read = "$AppDir$\\dics")]
        public static Dictionary LoadFromFile(string DictionaryLanguage)
        {
            string dictionaryFile = string.Format(@"{1}\dics\{0}.xml", DictionaryLanguage,
               Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6));
            if (!File.Exists(dictionaryFile))
            {
                throw new FileNotFoundException("Could Not Load Dictionary: " + dictionaryFile);
            }
            Dictionary dict = new Dictionary();
            XElement doc = XElement.Load(dictionaryFile);
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

            List<string> unimpwords = new List<string>();
            dict.UnimportantWords = new List<string>();
            unimpwords = LoadValueOnlySection(doc, "grader-tc");
            foreach (string unimpword in unimpwords)
            {
                dict.UnimportantWords.Add(unimpword);
            }
            return dict;
        }

        private static List<string> LoadValueOnlySection(XElement doc, string section)
        {
            List<string> list = new List<string>();
            IEnumerable<XElement> step1pre = doc.Elements(section);
            foreach (var x in step1pre.Elements())
            {
                list.Add(x.Value);
            }
            return list;
        }

        private static List<string> LoadValueOnlyRule(XElement doc, string section, string container)
        {
            List<string> list = new List<string>();
            IEnumerable<XElement> step1pre = doc.Elements(section).Elements(container);
            foreach (var x in step1pre.Elements())
            {
                list.Add(x.Value);
            }
            return list;
        }

        private static Dictionary<string, string> LoadKeyValueRule(XElement doc, string section, string container)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            IEnumerable<XElement> step1pre = doc.Elements(section).Elements(container);
            foreach (var x in step1pre.Elements())
            {
                string rule = x.Value;
                string[] keyvalue = rule.Split('|');
                if (!dictionary.ContainsKey(keyvalue[0]))
                    dictionary.Add(keyvalue[0], keyvalue[1]);
            }
            return dictionary;
        }
    }
}