namespace HRMSAPI.DTOs
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }
        public int DepartmentId { get; set; }

        public string Designation { get; set; }
        public int DesignationId { get; set; }

        public decimal Salary { get; set; }

        public string ProfileImage { get; set; }

        // NEW
        public int? ShiftId { get; set; }

        public string? ShiftName { get; set; }

    }
}