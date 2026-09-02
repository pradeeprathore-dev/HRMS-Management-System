namespace HRMSAPI.DTOs
{
    public class BellResponseDto
    {
        public int UnreadCount { get; set; }

        public List<NotificationResponseDto> LatestNotifications
        {
            get;
            set;
        }
        =
        new();
    }
}