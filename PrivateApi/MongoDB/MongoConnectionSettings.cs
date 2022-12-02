namespace PrivateApi.MongoDB
{
    public class MongoConnectionSettings : IMongoConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string WhiskyDatabaseName { get; set; }
        public string AdminDatabaseName { get; set; }

        public MongoConnectionSettings() { }

        public MongoConnectionSettings(string ConnectionString, string WhiskyDatabaseName, string AdminDatabaseName) 
        {
            this.ConnectionString = ConnectionString;
            this.WhiskyDatabaseName = WhiskyDatabaseName;
            this.AdminDatabaseName = AdminDatabaseName;
        }
    }

    public interface IMongoConnectionSettings
    {
        string ConnectionString { get; set; }
        string WhiskyDatabaseName { get; set; }
        string AdminDatabaseName { get; set; }
    }
}
