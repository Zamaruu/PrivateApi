namespace PrivateApi.MongoDB
{
    public class MongoConnectionSettings : IMongoConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public MongoConnectionSettings() { }
        public MongoConnectionSettings(string ConnectionString, string DatabaseName) 
        {
            this.ConnectionString = ConnectionString;
            this.DatabaseName = DatabaseName;
        }

    }

    public interface IMongoConnectionSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
