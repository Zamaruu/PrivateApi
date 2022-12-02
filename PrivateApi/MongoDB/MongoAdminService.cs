using MongoDB.Driver;
using PrivateApi.Data;
using PrivateApi.Data.ObjectModels.Whisky;

namespace PrivateApi.MongoDB
{
    public class MongoAdminService
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoAdminService(IMongoConnectionSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.AdminDatabaseName);
        }

        private IMongoCollection<T> GetCollection<T>(MongoAdminCollections collection)
        {
            switch (collection)
            {
                case MongoAdminCollections.AdminLogs:
                    return _database.GetCollection<T>("api-admin-logs");
                default:
                    Console.Error.WriteLine("Collection could not be found!");
                    return null;
            }
        }

        public async Task<bool> SaveDocument<T>(T data, MongoAdminCollections type)
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

        public async Task<dynamic> GetDocuments<T>(MongoAdminCollections type)
        {
            var collection = GetCollection<T>(type);

            try
            {
                var result = collection.AsQueryable().ToList();
                return result;

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return null;
            }

        }
    }
}
