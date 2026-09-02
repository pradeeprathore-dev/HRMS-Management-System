namespace HRMSAPI.DTOs
{
    public class CreateNotificationDto
    {
        public int EmployeeId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }
    }
}
