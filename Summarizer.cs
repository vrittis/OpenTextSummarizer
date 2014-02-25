using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;

namespace OpenTextSummarizer
{
    public class Summarizer
    {
        internal Summarizer()
        {
        }

        public static SummarizedDocument Summarize(SummarizerArguments args)
        {
            var CurrentSummarizer = new Summarizer();
            return CurrentSummarizer.InnerSummarize(args, new Grader(), new Highlighter(), new Stemmer());
        }

        internal Stemmer m_Stemmer { get; set; }

        internal SummarizedDocument InnerSummarize(SummarizerArguments args, Grader grader, Highlighter highlighter, Stemmer stemmer)
        {
            m_Stemmer = stemmer;
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
            grader.Grade(article);
            highlighter.Highlight(article, args);
            return CreateSummarizedDocument(article, args);
        }

        internal SummarizedDocument CreateSummarizedDocument(Article article, SummarizerArguments args)
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

        internal Article ParseFile(string fileName, SummarizerArguments args)
        {
            string text = LoadFile(fileName);
            return ParseDocument(text, args);
        }

        internal Article ParseDocument(string text, SummarizerArguments args)
        {
            Dictionary rules = Dictionary.LoadFromFile(args.DictionaryLanguage);
            Article article = new Article(rules, m_Stemmer);
            article.ParseText(text);
            return article;
        }

        [FileIOPermission(SecurityAction.Demand)]
        private string LoadFile(string fileName)
        {
            if (fileName != string.Empty)
                return File.ReadAllText(fileName);
            return string.Empty;
        }
    }
}