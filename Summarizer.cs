using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Permissions;

namespace OpenTextSummarizer
{
    public class Summarizer
    {
        static Summarizer() { }
        public static SummarizedDocument Summarize(SummarizerArguments args)
        {
            if (args == null) return null;
            Article article = null;
            if (args.InputString.Length > 0 && args.InputFile.Length == 0)
            {
                article = ParseDocument(args.InputString, args);
            }
            else
            {
                article = ParseFile(args.InputFile, args);
            }
            Grader.Grade(article);
            Highlighter.Highlight(article, args);
            SummarizedDocument sumdoc = CreateSummarizedDocument(article, args);
            return sumdoc;
            
        }

        private static SummarizedDocument CreateSummarizedDocument(Article article, SummarizerArguments args)
        {
            SummarizedDocument sumDoc = new SummarizedDocument();
            sumDoc.Concepts = article.Concepts;
            foreach (Sentence sentence in article.Sentences)
            {
                if (sentence.Selected)
                {
                    sumDoc.Sentences.Add(sentence.OriginalSentence);
                }
            }
            return sumDoc;
        }

        private static Article ParseFile(string fileName, SummarizerArguments args)
        {
            string text = LoadFile(fileName);
            return ParseDocument(text, args);
        }

        private static Article ParseDocument(string text, SummarizerArguments args)
        {
            Dictionary rules = Dictionary.LoadFromFile(args.DictionaryLanguage);
            Article article = new Article(rules);
            article.ParseText(text);
            return article;
        }

        [FileIOPermission(SecurityAction.Demand)]
        private static string LoadFile(string fileName)
        {
            if (fileName != string.Empty)
                return File.ReadAllText(fileName);
            return string.Empty;
        }
    }
}
