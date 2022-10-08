using PrivateApi.Data;
using PrivateApi.Data.ObjectModels.Whisky;
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

        public async Task UploadLinks(List<string> rawLinks)
        {
            if (rawLinks == null)
            {
                Console.Error.WriteLine("Links could not be uploaded to mongodb");
                return;
            }

            Console.WriteLine("Start link updating...");

            var skippedLinks = 0;

            foreach (var link in rawLinks)
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
        }
    }
}
