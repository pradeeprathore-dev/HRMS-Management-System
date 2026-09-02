namespace HRMSAPI.DTOs
{
    public class UpcomingHolidayDto
    {
        public int HolidayId { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsOptional { get; set; }
    }
}