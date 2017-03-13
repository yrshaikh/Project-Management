using App.Models;
using App.Models.User;
using App.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Services
{
    public class UserService : IUserService
    {
        private readonly MongoDbImplementation _database;
        public UserService()
        {
            _database = new MongoDbImplementation(TableNameConstants.User);
        }

        public bool Authenticate(string email, string password)
        {
            var filter = Builders<RegisterViewModel>.Filter.Or(
                            Builders<RegisterViewModel>.Filter.Where(p => p.Email.ToLower().Contains(email.ToLower())),
                            Builders<RegisterViewModel>.Filter.Where(p => p.Password.ToLower().Contains(password)));

            var document = _database.FindOne(filter);
            return document != null;
        }

        public bool IsEmailRegistered(string email)
        {
            var filter = new BsonDocument("Email", email);
            var document = _database.Find<RegisterViewModel>(filter);
            return document != null;
        }

        public void Register(RegisterViewModel model)
        {
            _database.Create(model);
        }

        public RegisterViewModel GetUser(string email)
        {
            var filter = Builders<RegisterViewModel>.Filter.Or(
                            Builders<RegisterViewModel>.Filter.Where(p => p.Email.ToLower().Contains(email.ToLower())));

            RegisterViewModel document = _database.FindOne(filter);
            return document;
        }

        public AuthenticatedUserModel GetUserDetails(string email)
        {
            RegisterViewModel userDocument = GetUser(email);
            return new AuthenticatedUserModel
            {
                FullName = userDocument.Name,
                Email = userDocument.Email
            };
        }
    }
}