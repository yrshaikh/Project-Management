using MongoDB.Driver;

namespace App.Mongo
{
    public abstract class AbstractMongoDatabase
    {
        protected readonly IMongoDatabase Database;
        protected readonly string TableName;
        protected AbstractMongoDatabase(string tableName)
        {
            var client = new MongoClient(MongoDbConfiguration.ConnectionString);
            Database = client.GetDatabase(MongoDbConfiguration.Database);
            TableName = tableName;
        }
    }
}