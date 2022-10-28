using MongoDB.Driver;
using PrivateApi.Data;
using PrivateApi.Data.ObjectModels.Whisky;

namespace PrivateApi.MongoDB
{
    public class MongoWhiskyService
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoWhiskyService(IMongoConnectionSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
        }

        private IMongoCollection<T> GetCollection<T>(MongoWhiskyCollections collection)
        {
            switch (collection)
            {
                case MongoWhiskyCollections.WhiskyDeLinks:
                    return _database.GetCollection<T>("whisky-de-links");
                case MongoWhiskyCollections.WhiskyLinkScrappingLogs:
                    return _database.GetCollection<T>("whisky-link-scrapping-logs");
                case MongoWhiskyCollections.WhiskyBottles:
                    return _database.GetCollection<T>("whisky-bottles");
                default:
                    Console.Error.WriteLine("Collection could not be found!");
                    return null;
            }
        }

        public async Task<bool> SaveDocument<T>(T data, MongoWhiskyCollections type)
        {
            var collection = GetCollection<T>(type);
            
            try
            {
                await collection.InsertOneAsync(data);
                return true;

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return true;
            }

        }

        public async Task<bool> LinkExists(string Link)
        {
            var collection = GetCollection<WhiskyDetailLink>(MongoWhiskyCollections.WhiskyDeLinks);
            var item = await collection.Find(wdl => wdl.OriginalLink.Equals(Link)).FirstOrDefaultAsync();

            return item != null;
        }

        public async Task<bool> BottleExists(string Link)
        {
            var collection = GetCollection<WhiskyBottleDetail>(MongoWhiskyCollections.WhiskyBottles);
            var item = await collection.Find(wbd => wbd.OriginalLink.Equals(Link)).FirstOrDefaultAsync();

            return item != null;
        }
    }
}
