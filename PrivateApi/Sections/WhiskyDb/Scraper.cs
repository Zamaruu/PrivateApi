using HtmlAgilityPack;
using PrivateApi.Data.ResponseModels;

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
        public async Task<LinkIndexingResponse> IndexLinks(int scrappingLimit)
        {
            int apiPage = 1;
            bool lastCallWasSuccess;
            List<string> scrappedLinks = new();

            Console.WriteLine("Starting Scraping...");
            
            try
            {
                do
                {
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine($"Page: {apiPage}");

                    var url = $"{BaseUrl}{apiPage}";
                    var response = await CallUrl(url);
                    lastCallWasSuccess = response.IsSuccessStatusCode;

                    if (response != null)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        var links = ParseHtml(body);

                        if (links != null) scrappedLinks.AddRange(links);
                        apiPage++;
                    }


                } while (lastCallWasSuccess && apiPage <= scrappingLimit);

                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine($"Scraping Finished. {scrappedLinks.Count} Links were fetched from {apiPage} Pages!");

                return new LinkIndexingResponse("https://whisky.de", scrappedLinks.Count, apiPage, scrappedLinks);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Site could not be fetched!\n" + e.ToString());
                return null;
            }
        }



        // Worker Methods

        private async Task<HttpResponseMessage> CallUrl(string fullUrl)
        {

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(fullUrl);
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
