namespace HRMSWEB.Models
{
    public class NotificationBellViewModel
    {
        public int UnreadCount { get; set; }

        public List<NotificationViewModel> LatestNotifications { get; set; } = new();

    }
}