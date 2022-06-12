using System.ComponentModel.DataAnnotations;

namespace ParkyWeb.Models
{
    public enum DifficultyType { Easy, Moderate, Difficult, Expert }
    public class Trail
    {
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; }

        public string Distance { get; set; }
        public DifficultyType Difficulty;


        [Required]
        public int NationalParkId { get; set; }
        public NationalPark NationalPark { get; set; }
    }
}
