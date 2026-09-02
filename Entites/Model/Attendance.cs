using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Attendance : BaseEntity
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public DateTime Date { get; set; }

        public DateTime PunchIn { get; set; }

        public DateTime? PunchOut { get; set; }

        public string Status { get; set; }

    }
}
