using System.ComponentModel.DataAnnotations;

namespace gameLib.Models
{
    public record Game
    {
        [Required]
        public string   Id      { get; init; }
        [Required]
        public string   Title   { get; init; }
        [Required]
        public string   Company { get; init; }
        [Required]
        public decimal  Price   { get; init; }
    }
}