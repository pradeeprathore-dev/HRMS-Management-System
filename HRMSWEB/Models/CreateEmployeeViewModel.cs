using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Models
{
    public class CreateEmployeeViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }

        public decimal Salary { get; set; }

        public IFormFile ImageFile { get; set; }

        public string ProfileImage { get; set; }
        public int ShiftId { get; set; }

        public List<SelectListItem> Shifts { get; set; } = new();


        // DROPDOWNS

        public List<SelectListItem> Departments
            = new();

        public List<SelectListItem> Designations
            = new();
    }
}
