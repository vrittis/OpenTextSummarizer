namespace OpenTextSummarizer.Demo
{
    public class WikipediaContentProvider : Interfaces.IContentProvider
    {
        private readonly string _wikipediaContent;

        public WikipediaContentProvider(string articleName)
        {
            var hapHtmlWeb = new HtmlAgilityPack.HtmlWeb();
            var htmlDocument = hapHtmlWeb.Load($"http://en.wikipedia.org/wiki/{articleName}");
            _wikipediaContent = htmlDocument.DocumentNode.SelectSingleNode("//div[@id=\"mw-content-text\"]").InnerText;
            int end = _wikipediaContent?.IndexOf("Sources[edit]") ?? -1;
            if (end > 0)
            {
                _wikipediaContent = _wikipediaContent?.Substring(0, end);
            }
        }

        public string Content => _wikipediaContent?.Replace("\n", "\r\n");
    }
}