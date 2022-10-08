using MongoDB.Bson.Serialization.Attributes;

namespace PrivateApi.Data.ResponseModels
{
    public class LinkIndexingResponse
    {
        [BsonId]
        public string Id { get; set; }

        /// <summary>
        /// The url from which the links were scraped. 
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Total count of the scrapped links.
        /// </summary>
        public int LinkCount { get; set; }

        /// <summary>
        /// Total count of the scrapped (api-)pages.
        /// </summary>
        public int ScrappedPages { get; set; }

        /// <summary>
        /// Total count of the links that were not saved in the mongodb.
        /// </summary>
        public int? SkippedLinks { get; set; } = 0;

        /// <summary>
        /// Total count of the links that were saved in the mongodb.
        /// </summary>
        public int? SavedLinks { get; set; } = 0;

        /// <summary>
        /// The Date the Scrapping occured
        /// </summary>
        public DateTime ScrapeDate { get; set; }

        [BsonIgnore]
        public List<string> Links { get; set; } = new();

        public LinkIndexingResponse() { }

        public LinkIndexingResponse(string BaseUrl, int LinkCount, int ScrappedPages, List<string> Links) 
        {
            Id = Guid.NewGuid().ToString();
            ScrapeDate = DateTime.Now.ToLocalTime();
            this.BaseUrl = BaseUrl;
            this.LinkCount = LinkCount;
            this.ScrappedPages = ScrappedPages;
            this.Links = Links;
        }

        public LinkIndexingResponse HttpResponse()
        {
            return new LinkIndexingResponse()
            {
                Id = Id,
                BaseUrl = BaseUrl,
                LinkCount = LinkCount,
                ScrapeDate = ScrapeDate,
                SavedLinks = SavedLinks,
                ScrappedPages=ScrappedPages,
                SkippedLinks=SkippedLinks,
            };
        }

        public override string ToString()
        {
            return @$"
                {Id}-{ScrapeDate}\n
                {ScrappedPages} pages were searched.\n
                {LinkCount} links were scrapped.\n
                Of these {LinkCount} links, {SavedLinks} links were saved and {SkippedLinks} links were skipped and ignored.\n 
            ";
        }
    }
}
