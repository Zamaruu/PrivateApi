using MongoDB.Bson.Serialization.Attributes;
using PrivateApi.Data.ObjectModels.Interfaces;

namespace PrivateApi.Data.ObjectModels.SubModels
{
    public class DistilleryData : IScrapBase
    {
        [BsonId]
        public string Id { get; set; }
        public DateTime ScrapDate { get; set; }
        public string OriginalLink { get; set; }
        public string Name { get; set; }

        public DistilleryData() { }
        public DistilleryData(string Link, string Name)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OriginalLink = Link;
            this.Name = Name;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
        public DistilleryData(string Id, string Link, string Name)
        {
            this.Id = Id;
            this.OriginalLink = Link;
            this.Name = Name;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
    }
}
