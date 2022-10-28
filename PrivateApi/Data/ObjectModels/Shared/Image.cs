namespace PrivateApi.Data.ObjectModels
{
    public class Image
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Image () { }

        public Image(string Title, string Url)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Title = Title;
            this.Url = Url;
        }

        public Image(string Title, string Url, int Width, int Height)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Title = Title;
            this.Url = Url;
            this.Width = Width;
            this.Height = Height;
        }
    }
}
