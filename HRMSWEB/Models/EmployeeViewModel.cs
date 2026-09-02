using Newtonsoft.Json;

namespace HRMSWEB.Models
{
    public class EmployeeViewModel
    {
        //[JsonProperty("id")]
        public int Id { get; set; }

        //[JsonProperty("firstName")]
        public string FirstName { get; set; }

        //[JsonProperty("lastName")]
        public string LastName { get; set; }

        //[JsonProperty("email")]
        public string Email { get; set; }

        //[JsonProperty("department")]
        public string Department { get; set; }

        //[JsonProperty("designation")]
        public string Designation { get; set; }

        //[JsonProperty("salary")]
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }
        public string ProfileImage { get; set; }
    }
}