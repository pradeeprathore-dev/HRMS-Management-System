using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class EmployeeDetails:BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
    }
}
