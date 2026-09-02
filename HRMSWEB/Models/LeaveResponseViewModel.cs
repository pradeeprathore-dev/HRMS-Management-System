namespace HRMSWEB.Models
{
    public class LeaveResponseViewModel
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }
    }
}