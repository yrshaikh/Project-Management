using System.Collections.Generic;
using App.Models.Project;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Mongo
{
    public class MongoDbImplementation : AbstractMongoDatabase
    {
        public MongoDbImplementation(string tableName) : base(tableName)
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

        public T FindOne<T>(FilterDefinition<T> filter)
        {
            var document = Database.GetCollection<T>(TableName).Find(filter).FirstOrDefault();
            return document;
        }

        public List<T> FindAll<T>(FilterDefinition<T> filter)
        {
            var document = Database.GetCollection<T>(TableName).Find(filter).ToList();
            return document;
        }
    }
}