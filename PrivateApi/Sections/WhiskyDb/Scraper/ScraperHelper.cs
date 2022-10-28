using HtmlAgilityPack;
using PrivateApi.Data.ObjectModels;
using PrivateApi.Data.ObjectModels.SubModels;
using PrivateApi.Data.ObjectModels.Whisky;

namespace PrivateApi.Sections.WhiskyDb
{
    public partial class WebScraper
    {
        private const string div = "div";
        private const string span = "span";
        private const string baseUrl = "https://www.whisky.de";

        #region HTML Parser
        private List<string>? ParseLinkHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            //var content = GetWhiskyContentSection(htmlDoc);
            //if (content == null) return null;

            var items = HtmlQueries.GetWhiskyItems(htmlDoc.DocumentNode);
            if (items == null) return null;

            List<string> links = new();

            foreach (var item in items)
            {
                var titleTag = item.Descendants(div)
                    .Where(node => node.GetClasses().Contains("title"))
                    .FirstOrDefault();

                var linkTag = titleTag.Descendants("a")
                    .FirstOrDefault();

                string hrefValue = linkTag.GetAttributeValue("href", string.Empty);
                links.Add($"https://www.whisky.de{hrefValue}");

                Console.WriteLine($"https://www.whisky.de{hrefValue}");
            }

            return links;
        }


        private WhiskyBottleDetail? ParseDetailHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var mainContentSection = htmlDoc.GetElementbyId("content-main");
            var rawDetailContent = mainContentSection.Descendants(div)
                    .Where(node => node.GetClasses().Contains("column-inner"))
                    .FirstOrDefault();

            if (rawDetailContent == null) return null;

            var bottle = BuildBottleDetailObject(rawDetailContent);

            return bottle;
        }


        #endregion

        #region Detail Scraper Helper

        private WhiskyBottleDetail BuildBottleDetailObject(HtmlNode detailContainer)
        {
            WhiskyBottleDetail whisky = new(string.Empty);

            whisky.Name = GetBottleDetailName(detailContainer);
            whisky.BottleDescription = GetBottleDescription(detailContainer);
            whisky.Images = GetBottleImageUrls(detailContainer);

            return whisky;
        }
        
        /// <summary>
        /// Reads the Heading from the Whisky HTML Doc
        /// </summary>
        /// <param name="container"></param>
        /// <returns>Concated Name and Age of the Whisky</returns>
        private string GetBottleDetailName(HtmlNode container)
        {
            var nameNode = container.Descendants(span)
                    .Where(node => node.GetClasses().Contains("marke"))
                    .FirstOrDefault();

            var ageNode = container.Descendants(span)
                    .Where(node => node.GetClasses().Contains("alterEtikett"))
                    .FirstOrDefault();

            if (nameNode == null && ageNode == null) return string.Empty;

            var name = nameNode.InnerText ?? string.Empty;
            var age = ageNode.InnerText ?? string.Empty;

            return $"{name.Trim()} {age.Trim()}".Trim();
        }


        /// <summary>
        /// Reads the Description from the Whisky HTML Doc
        /// </summary>
        /// <param name="container"></param>
        /// <returns>Cleaned up Description of the Whisky Bottle</returns>
        private string GetBottleDescription(HtmlNode container)
        {
            var descriptionNode = container.Descendants(div)
                    .Where(node => node.GetClasses().Contains("description") && node.GetClasses().Contains("visible-desktop"))
                    .FirstOrDefault();

            if (descriptionNode == null) return string.Empty;

            var rawDescription = descriptionNode.InnerText ?? string.Empty;
            var description = CleanUpScrapedString(rawDescription);

            return description;
        }

        /// <summary>
        /// Reads the Images and their Urls from the Whisky HTML Doc
        /// </summary>
        /// <param name="container"></param>
        /// <returns>List of Image Urls</returns>
        private WhiskyImage GetBottleImageUrls(HtmlNode container)
        {
            var whiskyImages = new WhiskyImage();

            var imageListNode = container.Descendants(div)
                   .Where(node => node.GetClasses().Contains("image-slider-elements"))
                   .FirstOrDefault();

            if (imageListNode == null) return new();

            var imageNodes = imageListNode.Descendants(div).Where(node => node.GetClasses().Contains("image-slider-element"));

            foreach (var imageElement in imageNodes)
            {
                var teaserImage = GetBottleTeaserImage(imageElement);
                var fullImage = GetBottleImage(imageElement);

                if(teaserImage != null) whiskyImages.TeaserImages.Add(teaserImage);
                if(fullImage != null) whiskyImages.Images.Add(fullImage);
            }

            return whiskyImages;
        }

        private Image? GetBottleTeaserImage(HtmlNode container)
        {
            var imageNode = container.Descendants("img").FirstOrDefault();

            if(imageNode == null) return null;

            string hrefValue = imageNode.GetAttributeValue("src", string.Empty);
            string title = imageNode.GetAttributeValue("title", string.Empty);
            string widthStr = imageNode.GetAttributeValue("width", string.Empty);
            string heightStr = imageNode.GetAttributeValue("height", string.Empty);

            var image = new Image(title, $"{baseUrl}{hrefValue}", int.Parse(widthStr), int.Parse(heightStr));

            return image;
        }

        private Image? GetBottleImage(HtmlNode container)
        {
            var fullImageContainer = container.Descendants(div)
                    .Where(node => node.GetClasses().Contains("zoomable"))
                   .FirstOrDefault();

            if (fullImageContainer == null) return null;

            string hrefValue = fullImageContainer.GetAttributeValue("data-zoom-image", string.Empty);
            hrefValue = $"{baseUrl}{hrefValue}";

            var image = new Image("Zoom Image", hrefValue);

            return image;
        }

        #endregion
    }
}
