using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class SupportTicket: BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Issue { get; set; }
        public string Status { get; set; }
    }
}
