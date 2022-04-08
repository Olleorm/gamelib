using System.Linq;
using System.Collections.Generic;

using MongoDB.Driver;

using gameLib.Models;

namespace gameLib.Repos
{
    public class MongoDBGamesRepository : GameDBInterface
    {
        // settings like "dbName", "collection", etc.. I would usually feed a program from outside so you dont need to change code in oreder to change settings
        // also makes it easier to fire up testing/staging environments if your into that sort of thing ;)
        private const string dbName = "library";
        private const string collection = "games";

        private readonly IMongoCollection<Game> gamesCollection; 

        public MongoDBGamesRepository(IMongoClient mongoClient)
        {
            IMongoDatabase db = mongoClient.GetDatabase(dbName);
            gamesCollection = db.GetCollection<Game>(collection);
        }

        public Game LoadByTitle(string title)
        {
            return gamesCollection.Find(game => game.Title == title).First();
        }

        public IEnumerable<Game> LoadAll(PageSettings pageSettings)
        {         
            return gamesCollection
                .Find(game => true) // all
                .Skip((pageSettings.PageNumber - 1) * pageSettings.PageSize) // offset to skip prev pages
                .Limit(pageSettings.PageSize) // all elements for the next page
                .ToList();
        }
    
        public IEnumerable<Game> Search(List<string> titles, PageSettings pageSettings)
        {
            return gamesCollection
                .Find(game => titles.Contains(game.Title))
                .Skip((pageSettings.PageNumber - 1) * pageSettings.PageSize) // offset to skip prev pages
                .Limit(pageSettings.PageSize) // all elements for the next page
                .ToList();
        }

        public bool Add(List<Game> games)
        {
            gamesCollection.InsertMany(games); // returns void. Does not seem to tell me if it succeds?
            return true;
        }
    }
}