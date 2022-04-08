using gameLib.Models;
using System.Linq;
using System.Collections.Generic;
using System;


namespace gameLib.Repos
{
    public class InMemGamesRepo : GameDBInterface
    {
        private readonly List<Game> games = new()
        {
            new Game { Id = System.Guid.NewGuid().ToString(), Title = "Focal Smash",    Price = 40, Company = "Lard Entertainment" },
            new Game { Id = System.Guid.NewGuid().ToString(), Title = "Mother",         Price = 20, Company = "Lard Entertainment" },
            new Game { Id = System.Guid.NewGuid().ToString(), Title = "Vindication 5",  Price = 15, Company = "Brosquad"           },
            new Game { Id = System.Guid.NewGuid().ToString(), Title = "Bones",          Price = 30, Company = "Lard Entertainment" },
        };

        public Game LoadByTitle(string title)
        {
            return games.Where(game => game.Title == title).First();
        }

        public IEnumerable<Game> LoadAll(PageSettings pageSettings)
        {
            return games
                .Where(game => true) // all
                .Skip((pageSettings.PageNumber - 1) * pageSettings.PageSize) // offset to skip prev pages
                .Take(pageSettings.PageSize) // all elements for the next page
                .ToList();
        }

        public IEnumerable<Game> Search(List<string> titles, PageSettings pageSettings)
        {
            return games
                .Where(game => titles.Contains(game.Title))
                .Skip((pageSettings.PageNumber - 1) * pageSettings.PageSize) // offset to skip prev pages
                .Take(pageSettings.PageSize) // all elements for the next page
                .ToList();
        }

        public bool Add(List<Game> games)
        {
            try
            {
                games.AddRange(games);
            }
            catch (System.ArgumentNullException e)
            {
                var error = e.ToString();
                Console.WriteLine($"failed to store game, Exeption: {error}");
                return false;
            }
            return true;
        }
    }
}