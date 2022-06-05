using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NationalParkDto

    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or members
}
