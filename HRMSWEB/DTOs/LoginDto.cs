namespace HRMSWEB.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
        public int EmployeeId { get; set; }

        public int RoleId { get; set; }
    }
}
