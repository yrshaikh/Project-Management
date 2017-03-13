using System.Collections.Generic;
using MongoDB.Bson;

namespace App.Models.Project
{
    public class ProjectListModel
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }

        public List<ProjectMembersModel> Members { get; set; }
    }
}