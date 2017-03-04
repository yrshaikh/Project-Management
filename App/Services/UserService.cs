using App.Models;
using MongoDB.Bson;
using MongoDatabase = App.Mongo.MongoDatabase;

namespace App.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDatabase _database;
        public UserService()
        {
             _database = new MongoDatabase(TableNameConstants.User);
        }

        public bool IsEmailRegistered(string email)
        {
            var filter = new BsonDocument("Email", email);
            var documents = _database.Find<RegisterViewModel>(filter);
            return documents != null;
        }

        public void Register(RegisterViewModel model)
        {
            _database.Create(model);
        }
    }
}