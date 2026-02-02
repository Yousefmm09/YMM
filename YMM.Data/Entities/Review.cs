using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YMM.Data.Entities.Identity;

namespace YMM.Data.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public int OrderId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        public string Comment { get; set; } = null!;

        public string? Images { get; set; } // JSON array of image URLs

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public string? RejectionReason { get; set; }

        public int HelpfulCount { get; set; } = 0;

        public int ReportCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        //[ForeignKey("OrderId")]
        //public virtual Order Order { get; set; } = null!;

        public virtual ICollection<ReviewHelpful> HelpfulMarks { get; set; } = new List<ReviewHelpful>();
    }
}
