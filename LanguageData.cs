using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Xml.Linq;

namespace OpenTextSummarizer
{
    public class LanguageData
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

        internal LanguageData() { }

        [FileIOPermission(SecurityAction.Demand, Read = "$AppDir$\\dics")]
        public static LanguageData LoadFromFile(string dictionaryLanguage)
        {
            string dictionaryFile = string.Format(@"{1}\dics\{0}.xml", dictionaryLanguage,
               Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6));

            if (!File.Exists(dictionaryFile))
            {
                throw new FileNotFoundException("Could Not Load LanguageData: " + dictionaryFile);
            }
            LanguageData dict = new LanguageData();
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

            dict.UnimportantWords = new List<string>();
            dict.UnimportantWords.AddRange(LoadValueOnlySection(doc, "grader-tc"));

            return dict;
        }

        private static List<string> LoadValueOnlySection(XElement doc, string section)
        {
            return doc.Elements(section)
                .Elements()
                .Select(element => element.Value)
                .ToList();
        }

        private static List<string> LoadValueOnlyRule(XElement doc, string section, string container)
        {
            return doc.Elements(section)
                .Elements(container)
                .Elements()
                .Select(element => element.Value)
                .ToList();

        }

        private static Dictionary<string, string> LoadKeyValueRule(XElement doc, string section, string container)
        {
            var retval = new Dictionary<string, string>();

            IEnumerable<XElement> docSectionElements = doc.Elements(section).Elements(container).Elements();
            foreach (var element in docSectionElements)
            {
                string[] values = element.Value.Split('|');
                if (!retval.ContainsKey(values[0]))
                {
                    retval.Add(values[0], values[1]);
                }
            }
            return retval;
        }
    }
}