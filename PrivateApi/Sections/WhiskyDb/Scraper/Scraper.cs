using HtmlAgilityPack;
using PrivateApi.Data.ObjectModels.Whisky;
using PrivateApi.Data.ResponseModels;
using System.Text;

namespace PrivateApi.Sections.WhiskyDb
{
    public partial class WebScraper
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
                        var links = ParseLinkHtml(body);

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


        public async Task<WhiskyBottleDetail?> IndexDetailForUri(Uri uri)
        {
            var response = await CallUrl(uri.ToString());
            
            var buff = await response.Content.ReadAsByteArrayAsync();
            var body = Encoding.GetEncoding("utf-8").GetString(buff);

            var result = ParseDetailHtml(body);

            if(result != null)
            {
                result.OriginalLink = uri.ToString();
            }

            return result;
        }

        // Worker Methods

        private async Task<HttpResponseMessage> CallUrl(string fullUrl)
        {

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(fullUrl);
            return response;
        }
    }
}
