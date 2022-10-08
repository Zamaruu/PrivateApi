using HtmlAgilityPack;

namespace PrivateApi.Sections.WhiskyDb
{
    public class WebScraper
    {
        private readonly string BaseUrl;

        public WebScraper(string BaseUrl)
        {
            this.BaseUrl = BaseUrl;
        }

        // Scraper
        public async Task<List<string>> IndexLinks()
        {

            Console.WriteLine("Starting Scraping...");

            var response = await CallUrl(BaseUrl);

            Console.WriteLine("Scraping Finished.");

            if (response != null)
            {
                var links = ParseHtml(response);
                // await UploadLinks(links);

                if(links != null) return links;
            }

            Console.Error.WriteLine("Site could not be fetched!");
            return new List<string>();
        }



        // Worker Methods

        private async Task<string> CallUrl(string fullUrl)
        {

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }



        private List<string>? ParseHtml(string html)
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

        private HtmlNode? GetWhiskyContentSection(HtmlDocument htmlDoc)
        {
            var contentSection = HtmlQueries.GetWhiskyContentSection(htmlDoc);
            var resultContainer = HtmlQueries.GetWhiskySearchResultContainer(contentSection);
            var contentItems = HtmlQueries.GetWhiskyResultListContainer(resultContainer);

            return contentItems;
        }
    }
}
