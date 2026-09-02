using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HRMSWEB.Models
{
    public class LeaveTypeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int DaysAllowed { get; set; }
        public int UsedDays { get; set; }
        public int RemainingDays { get; set; }
    }
}