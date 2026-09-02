namespace HRMSAPI.DTOs
{
    public class CreateHolidayDto
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public bool IsOptional { get; set; }
    }
}