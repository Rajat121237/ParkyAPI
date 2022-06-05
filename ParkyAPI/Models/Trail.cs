using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkyAPI.Models
{
    public enum DifficultyType { Easy, Moderate, Difficult, Expert }

    public class Trail
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Distance { get; set; }
        
        public DifficultyType Difficulty;

        [Required]
        public int NationalParkId { get; set; }

        [ForeignKey("NationalParkId")]
        public NationalPark NationalPark { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
