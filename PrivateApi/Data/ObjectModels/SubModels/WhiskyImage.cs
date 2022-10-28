using PrivateApi.Data.ObjectModels.Interfaces;

namespace PrivateApi.Data.ObjectModels.SubModels
{
    public class WhiskyImage : IScrapBase
    {
        public string Id { get; set; }
        public DateTime ScrapDate { get; set; }
        public string OriginalLink { get; set; }

        public List<Image> TeaserImages { get; set; }
        public List<Image> Images { get; set; }

        public WhiskyImage() 
        {
            this.Id = Guid.NewGuid().ToString();
            this.OriginalLink = string.Empty;
            ScrapDate = DateTime.Now.ToLocalTime();
            this.TeaserImages = new();
            this.Images = new();
        }
        
        public WhiskyImage(string Link, List<Image> TeaserImages, List<Image> Images)
        {
            this.Id = Guid.NewGuid().ToString();
            this.OriginalLink = Link;
            ScrapDate = DateTime.Now.ToLocalTime();
            this.TeaserImages = TeaserImages;
            this.Images = Images;

        }
    }
}
