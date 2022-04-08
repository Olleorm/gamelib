using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Newtonsoft.Json;

using client.GamePrinter;
using client.Entities;


namespace browserClient
{
    public class Program
    {
        public static readonly HttpClient httpClient = new HttpClient();
        static async Task Main()
        {
            Cli cli = new Cli();
            await cli.RunCli();
        }
    }

    public class Cli
    {
        private Client client = new Client("http://127.0.0.1:5000/games");
        private int defaultInitialPageNum = 1;
        private int defaultPageSize = 5;

        public async Task RunCli()
        {
            var run = true;
            while(run) {
                Console.WriteLine("help to display options");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "findTitle":
                        Console.Write("Title: ");
                        var title = Console.ReadLine();
                        await CallGetGameByTitle(title);
                        break;

                    case "list":
                        await CallGetAllGames();
                        break;

                    case "add":
                        Console.Write("path to games file: ");
                        var pathToGamesJson = Console.ReadLine();
                        await CallAddGames(pathToGamesJson);
                        break;

                    case "search":
                        Console.Write("path to title file: ");
                        var titles = Console.ReadLine();
                        await CallSearchGames(titles);
                        break;

                    case "help":
                        Console.WriteLine("\tfindTitle\tsearches for the game with the specified title");
                        Console.WriteLine("\tlist\t\tlists all games");
                        Console.WriteLine("\tadd\t\tadds all games from a specified json-file in the format: [{\"title\": \"<string>\", \"price\":<decimal>, \"company\":\"<string>\", \"id\": \"<string>\"}...]");
                        Console.WriteLine("\tsearch\t\tloads all games with the titles from specified json-file in the format: [\"<string>\"...]");
                        break;

                    case "exit":
                        run = false;
                        break;

                    default:
                        break;
                }
            }
        }

        private async Task CallGetGameByTitle(string title)
        {
            var game = await client.GetGameByTitle(title);
            if (game == null)
            {
                return;
            }
            GamePrinter.PrintGames(new List<Game> { game });
        }

        private async Task CallGetAllGames()
        {
            int pageNum = defaultInitialPageNum;
            int pageSize = 5;
            while (true)
            {
                var games = await client.GetAllGames(pageNum, pageSize);
                if (games.Count < 1)
                {
                    return;
                }
                GamePrinter.PrintGames(games);
                Console.ReadLine();
                pageNum++;
            }
        }

        private async Task CallSearchGames(string searchPath)
        {
            int pageNum = defaultInitialPageNum;
            int pageSize = defaultPageSize;
            while (true)
            {
                var titlesJson = JsonReader.ReadJson(searchPath);
                var games = await client.SearchGames(titlesJson, pageNum, pageSize);
                if (games.Count < 1)
                {
                    return;
                }
                GamePrinter.PrintGames(games);
                Console.ReadLine();
                
                pageNum++;
            }
        }

        private async Task CallAddGames(string savePath)
        {
            var gamesJson = JsonReader.ReadJson(savePath);
            await client.AddGames(gamesJson);
        }
    }

    public static class JsonReader
    {
        public static string ReadJson(string path)
        {
            return new StreamReader(path).ReadToEnd();
        }
    }

    public class Client
    {
        private string host;
        public Client(string host) { this.host = host; }


        public async Task<Game> GetGameByTitle(string title)
        {
            var req = new HttpRequestMessage(
                HttpMethod.Get,
                $"{host}/{title}"
            );
            var result = await Program.httpClient.SendAsync(req);
            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.StatusCode.ToString());
                return null;
            }

            string res = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Game>(res);
        }

        public async Task<List<Game>> GetAllGames(int pageNum, int pageSize)
        {
            HttpRequestMessage req = new HttpRequestMessage(
                HttpMethod.Get,
                $"{host}?pageNumber={pageNum}&pageSize={pageSize}"
            );

            var result = await Program.httpClient.SendAsync(req);
            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.StatusCode.ToString());
                return new List<Game>();
            }

            string res = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Game>>(res);
        }

        public async Task<List<Game>> SearchGames(string searchJson, int pageNum, int pageSize)
        {
            var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{host}/search?pageNumber={pageNum}&pageSize={pageSize}"
            );
            req.Content = new StringContent(
                searchJson,
                Encoding.UTF8,
                "application/json"
            );

            var result = await Program.httpClient.SendAsync(req);
            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.StatusCode.ToString());
                return new List<Game>() { };
            }

            string res = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Game>>(res);
        }

        public async Task<bool> AddGames(string gamesJson)
        {
            var req = new HttpRequestMessage(
                HttpMethod.Post,
                host
            );
            req.Content = new StringContent(
                gamesJson,
                Encoding.UTF8,
                "application/json"
            );

            var result = await Program.httpClient.SendAsync(req);
            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.StatusCode.ToString());
                return false;
            }
            return true;
        }
    }
}