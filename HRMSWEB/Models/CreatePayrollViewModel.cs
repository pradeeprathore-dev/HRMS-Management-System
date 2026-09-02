using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Models
{
    public class CreatePayrollViewModel
    {
        public int EmployeeId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        //public decimal HRA { get; set; }

        //public decimal Bonus { get; set; }

        public List<SelectListItem> Employees { get; set; }
            = new();
    }
}