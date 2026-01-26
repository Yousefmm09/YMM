using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Data.Entities.Identity
{
    public class UserPreferences
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;
        public bool PushNotifications { get; set; } = true;
        public bool OrderUpdates { get; set; } = true;
        public bool Promotions { get; set; } = true;
        public bool Newsletters { get; set; } = true;

        [MaxLength(10)]
        public string Language { get; set; } = "en";

        [MaxLength(10)]
        public string Currency { get; set; } = "EGP";

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
