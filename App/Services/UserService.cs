using App.Models;
using MongoDB.Bson;
using MongoDB.Driver;
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

        public bool Authenticate(string email, string password)
        {
            var filter = Builders<RegisterViewModel>.Filter.Or(
                            Builders<RegisterViewModel>.Filter.Where(p=>p.Email.ToLower().Contains(email.ToLower())),
                            Builders<RegisterViewModel>.Filter.Where(p => p.Password.ToLower().Contains(password)));
            
            var documents = _database.Find(filter);
            return documents != null;
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