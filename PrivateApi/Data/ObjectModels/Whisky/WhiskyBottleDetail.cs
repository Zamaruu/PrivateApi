using MongoDB.Bson.Serialization.Attributes;
using PrivateApi.Data.ObjectModels.Interfaces;
using PrivateApi.Data.ObjectModels.SubModels;

namespace PrivateApi.Data.ObjectModels.Whisky
{
    public class WhiskyBottleDetail : IScrapBase
    {
        [BsonId]
        public string Id { get; set; }
        public string OriginalLink { get; set; }
        public DateTime ScrapDate { get; set; }


        #region Identifier & General Information

        public string Name { get; set; }
        public string BrandName { get; set; }
        public string BottleAge { get; set; }

        // TODO: Bottle Reating

        public List<string> ImageUrls { get; set; }

        public string BottleDescription { get; set; }

        #endregion

        #region Bottle Details

        public DistilleryData Distillery { get; set; }
        public WhiskyCountry Country { get; set; }
        public WhiskyType Type { get; set; }
        public int AlcoholLevel { get; set; }
        public float BottleSize { get; set; }

        #endregion


        public WhiskyBottleDetail() { }

        public WhiskyBottleDetail(string Link)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }

        public WhiskyBottleDetail(string Link, string Name, string BrandName, string BottleAge, List<string> ImageUrls, string BottleDescription, DistilleryData Distillery, WhiskyCountry Country, WhiskyType Type, int AlcoholLevel, float BottleSize)
        {
            this.Id = Guid.NewGuid().ToString();
            OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();

        }
        public WhiskyBottleDetail(string Id, string Link)
        {
            this.Id = Id;
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
    }
}
