using HtmlAgilityPack;

namespace PrivateApi.Sections.WhiskyDb
{
    public partial class WebScraper
    {
        #region HTML Parser
        private List<string>? ParseLinkHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            //var content = GetWhiskyContentSection(htmlDoc);
            //if (content == null) return null;

            var items = HtmlQueries.GetWhiskyItems(htmlDoc.DocumentNode);
            if (items == null) return null;

            List<string> links = new();

            foreach (var item in items)
            {
                var titleTag = item.Descendants("div")
                    .Where(node => node.GetClasses().Contains("title"))
                    .FirstOrDefault();

                var linkTag = titleTag.Descendants("a")
                    .FirstOrDefault();

                string hrefValue = linkTag.GetAttributeValue("href", string.Empty);
                links.Add($"https://www.whisky.de{hrefValue}");

                Console.WriteLine($"https://www.whisky.de{hrefValue}");
            }

            return links;
        }


        private string? ParseDetailHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var mainContentSection = htmlDoc.GetElementbyId("content-main");
            var rawDetailContetn = mainContentSection.Descendants("div")
                    .Where(node => node.GetClasses().Contains("column-inner"))
                    .FirstOrDefault();

            return "";
        }
        #endregion
    }
}
