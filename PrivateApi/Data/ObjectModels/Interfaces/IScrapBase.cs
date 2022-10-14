namespace PrivateApi.Data.ObjectModels.Interfaces
{
    public interface IScrapBase
    {
        public string Id { get; set; }
        public DateTime ScrapDate { get; set; }
        public string OriginalLink { get; set; }
    }
}
