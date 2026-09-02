namespace HRMSWEB.Models
{
    public class AttendanceViewModel
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public DateTime Date { get; set; }

        public DateTime PunchIn { get; set; }

        public DateTime? PunchOut { get; set; }

        public string Status { get; set; }
        public int WorkingMinutes { get; set; }
    }
}