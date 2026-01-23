using YMM.Application.Dto.Common;

namespace YMM.Application.Dto.Notification
{
    public class NotificationFilterDto : PaginationParams
    {
        public bool? UnreadOnly { get; set; }
        public string? Type { get; set; }
    }
}
