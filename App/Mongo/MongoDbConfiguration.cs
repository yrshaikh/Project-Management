using System.Configuration;

namespace App.Mongo
{
    public class MongoDbConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["mongodb:config:connectionstring"];
            }
        }

        public static string Database
        {
            get
            {
                return ConfigurationManager.AppSettings["mongodb:config:database"];
            }
        } 
    }
}