using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Models
{
    public class LeaveRequestViewModel
    {
        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public List<SelectListItem> LeaveTypes
            = new();
    }
}