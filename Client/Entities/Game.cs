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