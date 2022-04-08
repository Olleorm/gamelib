using System.Text;
using System.Collections;

namespace client.Entities
{
    public record Game
    {
        public string   Id      { get; init; }
        public string   Title   { get; init; }
        public string   Company { get; init; }
        public decimal  Price   { get; init; }
    }
}

namespace client.GamePrinter
{
using client.Entities;

        public struct PGame
    {
        public string Title   { get; init; }
        public string Price   { get; init; }
        public string Company { get; init; }
        public string Id      { get; init; }
    
        public PGame(string title, string price, string company, string id)
        {
            Title = title;
            Price = price;
            Company = company;
            Id = id;
        }
    }

    public static class GamePrinter
    {
        public static void PrintGames(List<Game> games)
        {
            List<PGame> pGame = new List<PGame>();
            foreach (var game in games)
            {
                pGame.Add(ToPGame(game));
            }

            foreach (var game in FormatForPrint(pGame))
            {
                Console.WriteLine(game);
            }
        }

        private static PGame ToPGame(Game game)
        { 
            return new PGame{
                Title = game.Title,
                Price = game.Price.ToString(),
                Company = game.Company,
                Id = game.Id
            };
        }

        private static List<string> FormatForPrint(List<PGame> games)
        {
            var headers = new PGame("Title", "Price", "Company", "Id");
            var emptyRow = new PGame("", "", "", "");
            games.Insert(0, headers);
            games.Insert(1, emptyRow);

            int longestId = 0;
            int longestCompany = 0;
            int longestTitle = 0;
            int longestPrice = 0;
            foreach (var game in games)
            {
                longestTitle = findLongest(longestTitle, game.Title);
                longestPrice = findLongest(longestPrice, game.Price);
                longestCompany = findLongest(longestCompany, game.Company);
                longestId = findLongest(longestId, game.Id);
            }

            List<string> formatedStrings = new List<string>();
            foreach (var game in games)
            {
                string formated = "| ";
                formated += AddPadding(longestTitle, game.Title);
                formated += AddPadding(longestPrice, game.Price);
                formated += AddPadding(longestCompany, game.Company);
                formated += AddPadding(longestId, game.Id);
                formatedStrings.Add(formated);
            }
            return formatedStrings;
        }

        private static int findLongest(int longest, string str)
        {
            if (longest < str.Length)
            {
                return str.Length;
            }
            return longest;
        }

        private static string AddPadding(int longest, string str)
        {
            if (longest < str.Length)
            {
                return str + " | ";
            }
            return  str + Repeat(" ", longest - str.Length) + " | ";
        }

        private static string Repeat(string value, int count)
        {
            return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
        }
    }
}