namespace YMM.Application.Dto.Notification
{
    public class NotificationPreferencesDto
    {
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;
        public bool PushNotifications { get; set; } = true;
        public bool OrderUpdates { get; set; } = true;
        public bool Promotions { get; set; } = false;
        public bool Newsletters { get; set; } = true;
    }

    public class UpdateNotificationPreferencesDto
    {
        public bool? EmailNotifications { get; set; }
        public bool? SmsNotifications { get; set; }
        public bool? PushNotifications { get; set; }
        public bool? OrderUpdates { get; set; }
        public bool? Promotions { get; set; }
        public bool? Newsletters { get; set; }
    }
}
