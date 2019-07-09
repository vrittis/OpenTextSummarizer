//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OpenTextSummarizer.Tests.Factories
//{
//    public class LanguageDataFactory
//    {
//        internal static LanguageData Build()
//        {
//            return new OpenTextSummarizer.LanguageData()
//            {
//                DepreciateValueRule = new List<string>(),
//                Language = "en",
//                LinebreakRules = new List<string>(),
//                ManualReplacementRules = new LanguageData<string, string>(),
//                NotALinebreakRules = new List<string>(),
//                PrefixRules = new LanguageData<string, string>(),
//                Step1PrefixRules = new LanguageData<string, string>(),
//                Step1SuffixRules = new LanguageData<string, string>(),
//                SuffixRules = new LanguageData<string, string>(),
//                SynonymRules = new LanguageData<string, string>(),
//                TermFreqMultiplierRule = new List<string>(),
//                UnimportantWords = new List<OpenTextSummarizer.Word>()
//            };
//        }
//    }
//}