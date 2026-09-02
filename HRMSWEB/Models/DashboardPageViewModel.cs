namespace HRMSWEB.Models
{
    public class DashboardPageViewModel
    {
        public DashboardViewModel Summary { get; set; }
            = new();

        public LeaveChartViewModel LeaveChart { get; set; }
            = new();

        public AttendanceChartViewModel AttendanceChart { get; set; }
            = new();

        public List<RecentLeaveViewModel> RecentLeaves { get; set; }
            = new();
        public List<RecentEmployeeViewModel> RecentEmployees { get; set; } = new();
        public List<UpcomingHolidayViewModel> UpcomingHolidays
        {
            get;
            set;
        }
=
new();
    }
}