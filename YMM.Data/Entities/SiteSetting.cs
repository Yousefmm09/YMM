using System;
using System.ComponentModel.DataAnnotations;

namespace YMM.Data.Entities
{
    public class SiteSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Key { get; set; } = null!;

        [Required]
        public string Value { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
