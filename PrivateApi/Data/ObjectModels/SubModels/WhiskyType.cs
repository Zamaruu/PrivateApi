using PrivateApi.Data.ObjectModels.Interfaces;

namespace PrivateApi.Data.ObjectModels.SubModels
{
    public class WhiskyType : IScrapBase
    {
        public string Id { get; set; }
        public DateTime ScrapDate { get; set; }
        public string OriginalLink { get; set; }

        public string Name { get; set; }
        public string ImageLink { get; set; }
        public string Descritption { get; set; }

        public WhiskyType() { }
        public WhiskyType(string Link)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
        public WhiskyType(string Id, string Link)
        {
            this.Id = Id;
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
        }
    }
}
