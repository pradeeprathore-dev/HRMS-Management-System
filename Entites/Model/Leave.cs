using Entites.Model;

namespace HRMSAPI.Model
{
    public class Leave: BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string Reason { get; set; }
    }
}
