using MongoDB.Bson.Serialization.Attributes;
using PrivateApi.Data.ObjectModels.Interfaces;

namespace PrivateApi.Data.ObjectModels.Whisky
{
    public class WhiskyDetailLink : IScrapBase
    {
        [BsonId]
        public string Id { get; set; }
        public string OriginalLink { get; set; }
        public DateTime ScrapDate { get; set; }

        public WhiskyDetailLink() { }
        public WhiskyDetailLink(string Link) 
        {
            this.Id = Guid.NewGuid().ToString();
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
        public WhiskyDetailLink(string Id, string Link)
        {
            this.Id = Id;
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
    }
}
