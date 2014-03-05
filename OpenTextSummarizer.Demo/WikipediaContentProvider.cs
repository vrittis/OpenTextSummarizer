using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTextSummarizer.Demo
{
    public class WikipediaContentProvider : OpenTextSummarizer.Interfaces.IContentProvider
    {
        private string wikipediaContent = string.Empty;

        public WikipediaContentProvider(string ArticleName)
        {
            var hapHtmlWeb = new HtmlAgilityPack.HtmlWeb();
            var htmlDocument = hapHtmlWeb.Load(string.Format("http://en.wikipedia.org/wiki/{0}", ArticleName));
            wikipediaContent = htmlDocument.DocumentNode.SelectSingleNode("//div[@id=\"mw-content-text\"]").InnerText;
        }

        public string Content
        {
            get { return wikipediaContent.Replace("\n", "\r\n"); }
        }
    }
}