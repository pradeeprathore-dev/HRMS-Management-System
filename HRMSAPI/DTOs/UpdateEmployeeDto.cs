using System.ComponentModel.DataAnnotations;

namespace HRMSAPI.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        [Range(1, int.MaxValue)]
        public int DesignationId { get; set; }

        [Range(1, 1000000)]
        public decimal Salary { get; set; }
        public int? ShiftId { get; set; }
    }
}