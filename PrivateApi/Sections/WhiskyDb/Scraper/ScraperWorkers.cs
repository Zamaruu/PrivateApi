namespace PrivateApi.Sections.WhiskyDb
{
    public partial class WebScraper
    {
        private string CleanUpScrapedString(string text)
        {
            // Linebreaks
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\t", string.Empty);
            text = text.Replace("\n", string.Empty);

            // Umlauts
            text = text.Replace("&szlig;", "ß");
            text = text.Replace("&auml;", "ä");
            text = text.Replace("&ouml;", "ö"); 
            
            return text;
        }
    }
}
