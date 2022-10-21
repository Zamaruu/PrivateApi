using HtmlAgilityPack;
using PrivateApi.Data.ObjectModels.Whisky;

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


        private WhiskyBottleDetail? ParseDetailHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var mainContentSection = htmlDoc.GetElementbyId("content-main");
            var rawDetailContent = mainContentSection.Descendants("div")
                    .Where(node => node.GetClasses().Contains("column-inner"))
                    .FirstOrDefault();

            if (rawDetailContent == null) return null;

            var bottle = BuildBottleDetailObject(rawDetailContent);

            return bottle;
        }


        #endregion

        #region Detail Scraper Helper

        private WhiskyBottleDetail BuildBottleDetailObject(HtmlNode detailContainer)
        {
            WhiskyBottleDetail whisky = new(string.Empty);

            whisky.Name = GetBottleDetailName(detailContainer);

            return whisky;
        }
        
        /// <summary>
        /// Reads the Heading from the Whisky HTML Doc
        /// </summary>
        /// <param name="container"></param>
        /// <returns>Concated Name and Age of the Whisky</returns>
        private string GetBottleDetailName(HtmlNode container)
        {
            var nameNode = container.Descendants("span")
                    .Where(node => node.GetClasses().Contains("marke"))
                    .FirstOrDefault();

            var ageNode = container.Descendants("span")
                    .Where(node => node.GetClasses().Contains("alterEtikett"))
                    .FirstOrDefault();

            if (nameNode == null && ageNode == null) return string.Empty;

            var name = nameNode.InnerText ?? "";
            var age = ageNode.InnerText ?? "";

            return $"{name.Trim()} {age.Trim()}".Trim();
        }


        #endregion
    }
}
