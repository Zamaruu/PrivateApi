using MongoDB.Bson.Serialization.Attributes;

namespace PrivateApi.Data.ResponseModels
{
    public class BottleScrappingResponse
    {
        [BsonId]
        public string Id { get; set; }

        /// <summary>
        /// Total count of the scrapped whisky bottles.
        /// </summary>
        public int ScrappedBottles { get; set; }

        /// <summary>
        /// Total count of the whisky bottles that were not saved in the mongodb.
        /// </summary>
        public int? SkippedBottles { get; set; } = 0;

        /// <summary>
        /// Total count of the whisky bottles that were saved in the mongodb.
        /// </summary>
        public int? SavedBottles { get; set; } = 0;

        /// <summary>
        /// The Date the Scrapping occured.
        /// </summary>
        public DateTime ScrapeDate { get; set; }

        public BottleScrappingResponse() { }

        public BottleScrappingResponse(int ScrappedBottles, int SkippedBottles, int SavedBottles) 
        {
            Id = Guid.NewGuid().ToString();
            ScrapeDate = DateTime.Now.ToLocalTime();
            this.ScrappedBottles = ScrappedBottles;
            this.SkippedBottles = SkippedBottles;
            this.SavedBottles = SavedBottles;
        }

        public override string ToString()
        {
            return @$"
                {Id}-{ScrapeDate}\n
                {ScrappedBottles} whisky bottles were scrapped.\n
                Of these {ScrappedBottles} bottles, {SavedBottles} bottles were saved and {SkippedBottles} bottles were skipped and ignored.\n 
            ";
        }
    }
}
