using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class EmployeeProject : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        // Extra fields (real-world use)
        public DateTime AssignedDate { get; set; }
        public string RoleInProject { get; set; }
    }
}
