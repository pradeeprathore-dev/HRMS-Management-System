namespace HRMSWEB.Models
{
    public class RecentEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public string ProfileImage { get; set; } = string.Empty;

        public DateTime JoinedOn { get; set; }
    }
}