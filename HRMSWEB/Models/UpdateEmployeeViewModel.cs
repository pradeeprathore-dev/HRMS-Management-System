using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Models
{
    public class UpdateEmployeeViewModel
    {
        public int ShiftId { get; set; }

        public List<SelectListItem> Shifts { get; set; } = new();
    }
}
