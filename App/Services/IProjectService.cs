using System.Collections.Generic;
using System.Linq;
using App.Models;
using App.Models.Project;
using App.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace App.Services
{
    public interface IProjectService
    {
        ObjectId Create(ProjectModel model);
        void AddMember(ObjectId id, RegisterViewModel owner);
        List<ProjectListModel> Get(string email);
    }

    public class ProjectService : IProjectService
    {
        private readonly MongoDbImplementation _database;
        public ProjectService()
        {
            _database = new MongoDbImplementation(TableNameConstants.Projects);
        }

        public ObjectId Create(ProjectModel model)
        {
            _database.Create(model);
            return model.Id;
        }

        public void AddMember(ObjectId id, RegisterViewModel owner)
        {
            var filter = Builders<ProjectModel>.Filter.Where(x => x.Id == id);
            ProjectModel project = _database.FindOne(filter);
            if (project.Members == null)
                project.Members = new List<RegisterViewModel>();

            project.Members.Add(owner);
        }

        public List<ProjectListModel> Get(string email)
        {
            var filter = Builders<ProjectModel>.Filter.Where(x => x.Members.Any(y => y.Email == email));
            List<ProjectModel> projects = _database.FindAll(filter);

            List<ProjectListModel> listing = new List<ProjectListModel>();
            foreach (ProjectModel project in projects)
            {
                ProjectListModel p = new ProjectListModel
                {
                    Id = project.Id,
                    Title = project.Title,
                    Members = new List<ProjectMembersModel>()
                };
                foreach (var member in project.Members)
                {
                    p.Members.Add(new ProjectMembersModel
                    {
                        Email = member.Email,
                        Name = member.Name,
                    });
                }
                listing.Add(p);
            }
            return listing;
        }
    }
}