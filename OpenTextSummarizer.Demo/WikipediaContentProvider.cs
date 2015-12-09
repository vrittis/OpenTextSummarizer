using HtmlAgilityPack;
using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer.Demo
{
    public class WikipediaContentProvider : IContentProvider
    {
        private readonly string _wikipediaContent;

        public WikipediaContentProvider(string articleName)
        {
            var hapHtmlWeb = new HtmlWeb();
            var htmlDocument = hapHtmlWeb.Load($"http://en.wikipedia.org/wiki/{articleName}");
            _wikipediaContent = htmlDocument.DocumentNode.SelectSingleNode("//div[@id=\"mw-content-text\"]").InnerText;
        }

        public string Content => _wikipediaContent.Replace("\n", "\r\n");
    }
}