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

        /// <summary>
        /// Pageinate the whiskybottle list to get better memory management
        /// </summary>
        /// <param name="list"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<T> GetPage<T>(IList<T> list, int page, int pageSize = 20)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
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

        public async Task<bool> UploadBottle(WhiskyBottleDetail bottle)
        {
            if (bottle == null)
            {
                Console.Error.WriteLine("Bottle could not be uploaded to mongodb");
                return false;
            }

            var bottleExists = await mongoService.BottleExists(bottle.OriginalLink);
            if (bottleExists)
            {
                Console.Error.WriteLine("Bottle could not be uploaded to mongodb because it already exists.");
                return false;
            }

            var result = await mongoService.SaveDocument(bottle, MongoWhiskyCollections.WhiskyBottles);

            if (!result)
            {
                Console.Error.WriteLine($"{bottle.Name} could not be uploaded to mongodb");
            }

            return true;
        }

        #region Logs
        public async Task<bool> UploadLinkIndexLog(LinkIndexingResponse log)
        {
            return await mongoService.SaveDocument(log, MongoWhiskyCollections.WhiskyLinkScrappingLogs);
        }

        public async Task<bool> UploadWhiskyScrappingLog(BottleScrappingResponse log)
        {
            return await mongoService.SaveDocument(log, MongoWhiskyCollections.WhiskyBottleScrappingLogs);
        }
        #endregion

        #region Getter

        public async Task<List<WhiskyBottleDetail>> GetWhiskyBottles()
        {
            var bottles = await mongoService.GetDocuments<WhiskyBottleDetail>(MongoWhiskyCollections.WhiskyBottles);
            return bottles;
        }

        public async Task<List<WhiskyDetailLink>> GetWhiskyDetailLinks()
        {
            var links = await mongoService.GetDocuments<WhiskyDetailLink>(MongoWhiskyCollections.WhiskyDeLinks);
            return links;
        }

        #endregion
    }
}
