using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App.Models.Project
{
    public class ProjectModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public List<RegisterViewModel> Members { get; set; } 
    }
}