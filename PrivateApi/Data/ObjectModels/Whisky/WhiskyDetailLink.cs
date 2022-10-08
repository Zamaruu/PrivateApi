using MongoDB.Bson.Serialization.Attributes;


namespace PrivateApi.Data.ObjectModels.Whisky
{
    public class WhiskyDetailLink
    {
        [BsonId]
        public string Id { get; set; }
        public string Link { get; set; }
        public DateTime ScrapDate { get; set; }

        public WhiskyDetailLink() { }
        public WhiskyDetailLink(string Link) 
        {
            this.Id = Guid.NewGuid().ToString();
            this.Link = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
        public WhiskyDetailLink(string Id, string Link)
        {
            this.Id = Id;
            this.Link = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
    }
}
