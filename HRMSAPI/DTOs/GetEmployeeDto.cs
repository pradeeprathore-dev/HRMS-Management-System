namespace HRMSAPI.DTOs
{
    public class GetEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public decimal Salary { get; set; }
    }
}
