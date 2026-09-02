namespace HRMSWEB.Models
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }

        public int TotalDepartments { get; set; }

        public int PendingLeaves { get; set; }

        public int PresentToday { get; set; }

        public int LateToday { get; set; }

        public int AbsentToday { get; set; }

        public int PendingPayrolls { get; set; }

        public string TodayHoliday { get; set; }
    }
}
