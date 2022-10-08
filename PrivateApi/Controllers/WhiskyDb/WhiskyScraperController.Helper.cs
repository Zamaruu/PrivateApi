using PrivateApi.Data;
using PrivateApi.Data.ObjectModels.Whisky;
using PrivateApi.Data.ResponseModels;
using PrivateApi.MongoDB;

namespace PrivateApi.Controllers.WhiskyDb
{
    public class WhiskyScraperControllerHelper
    {
        private readonly MongoWhiskyService mongoService;

        public WhiskyScraperControllerHelper(MongoWhiskyService mongoService)
        {
            this.mongoService = mongoService;
        }

        public async Task<LinkIndexingResponse> UploadLinks(LinkIndexingResponse fetchedLinksResponse)
        {
            if (fetchedLinksResponse == null)
            {
                Console.Error.WriteLine("Links could not be uploaded to mongodb");
                return null;
            }

            Console.WriteLine("Start link updating...");

            var skippedLinks = 0;

            foreach (var link in fetchedLinksResponse.Links)
            {
                if (await mongoService.LinkExists(link))
                {
                    skippedLinks++;
                    continue;
                }

                var newLink = new WhiskyDetailLink(link);
                var result = await mongoService.SaveDocument(newLink, MongoWhiskyCollections.WhiskyDeLinks);

                if (!result)
                {
                    Console.Error.WriteLine($"{link} could not be uploaded to mongodb");
                }
            }

            Console.WriteLine($"Links successfully updated, {skippedLinks} Links were skipped.");

            fetchedLinksResponse.SkippedLinks = skippedLinks;
            fetchedLinksResponse.SavedLinks = fetchedLinksResponse.LinkCount - skippedLinks;
            
            return fetchedLinksResponse;
        }

        #region Logs
        public async Task<bool> UploadLinkIndexLog(LinkIndexingResponse log)
        {
            return await mongoService.SaveDocument(log, MongoWhiskyCollections.WhiskyLinkScrappingLogs);
        }
        #endregion
    }
}
