// using MongoDB.Driver;

namespace gameLib.MongoSettings
{
    public class MongoDbSettings
    {
        public string   Host        { get; set; }
        public int      Port        { get; set; }
        public string   Connection { get {return $"mongodb://{Host}:{Port}"; }
        }
    }
}
