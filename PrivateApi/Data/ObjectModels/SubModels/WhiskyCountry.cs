using PrivateApi.Data.ObjectModels.Interfaces;

namespace PrivateApi.Data.ObjectModels.SubModels
{
    public class WhiskyCountry : IScrapBase
    {
        public string Id { get; set; }
        public DateTime ScrapDate { get; set; }
        public string OriginalLink { get; set; }

        public string CountryName { get; set; }
        public string Region { get; set; }

        public WhiskyCountry() { }
        public WhiskyCountry(string Link)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
        public WhiskyCountry(string Id, string Link)
        {
            this.Id = Id;
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
    }
}
