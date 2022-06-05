using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class TrailUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Distance { get; set; }

        public DifficultyType Difficulty;

        [Required]
        public int NationalParkId { get; set; }
    }
}
