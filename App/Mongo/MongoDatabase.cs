using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Mongo
{
    public class MongoDatabase : AbstractMongoDatabase
    {
        public MongoDatabase(string tableName) : base(tableName)
        {
        }

        public void Create<T>(T item)
        {
            Database.GetCollection<T>(TableName).InsertOne(item);
        }

        public T Find<T>(BsonDocument filter)
        {
            var document = Database.GetCollection<T>(TableName).Find(filter).FirstOrDefault();
            return document;
        }
    }
}