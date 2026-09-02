namespace HRMSWEB.Models
{
    public class HolidayViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public bool IsOptional { get; set; }
    }
}