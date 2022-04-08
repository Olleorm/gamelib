using gameLib.Models;
using System.Collections.Generic;

namespace gameLib.Repos
{
    public interface GameDBInterface
    {
        Game LoadByTitle(string title);
        IEnumerable<Game> LoadAll(PageSettings pageSettings);
        IEnumerable<Game> Search(List<string> titles, PageSettings pageSettings);
        bool Add(List<Game> games);
    }
}