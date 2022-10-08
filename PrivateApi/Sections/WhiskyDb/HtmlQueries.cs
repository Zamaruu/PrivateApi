using HtmlAgilityPack;

namespace PrivateApi.Sections.WhiskyDb
{
    public class HtmlQueries
    {
        public static HtmlNode? GetWhiskyContentSection(HtmlDocument htmlDoc)
        {
            if(htmlDoc == null) return null;

            var content = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetClasses().Contains("content") && node.GetClasses().Contains("main"))
                .FirstOrDefault();

            return content;            
        }

        public static HtmlNode? GetWhiskySearchResultContainer(HtmlNode content)
        {
            if (content == null) return null;

            var resultContainer = content.Descendants("div")
                  .Where(node => node.GetClasses().Contains("search-result-container"))
                  .FirstOrDefault();

            return resultContainer;
        }

        public static HtmlNode? GetWhiskyResultListContainer(HtmlNode resultContainer)
        {
            var resultList = resultContainer.Descendants("div")
                        .Where(node => node.GetClasses().Contains("resultlist") && node.GetClasses().Contains("flaschen"))
                        .FirstOrDefault();

            return resultList;
        }

        public static List<HtmlNode> GetWhiskyItems(HtmlNode itemNode)
        {
            return itemNode.Descendants("div")
                    .Where(node => node.GetClasses().Contains("item") && node.GetClasses().Contains("item-wrap"))
                    .ToList();
        }
    }
}
