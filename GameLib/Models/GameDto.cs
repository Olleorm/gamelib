namespace gameLib.Dtos
{    
    public record GameDto
    {
        public string   Id      { get; init; }
        public string   Title   { get; init; }
        public string   Company { get; init; }
        public decimal  Price   { get; init; }
    }
}

namespace gameLib
{
    using gameLib.Models;
    using gameLib.Dtos;

    public static class GameDtoHelpers
    {
        public static GameDto AsDto(this Game game) {
            return new GameDto{
                Id = game.Id,
                Title = game.Title,
                Company = game.Company,
                Price = game.Price
            };
        }
    }
}