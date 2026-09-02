using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class LeaveType : BaseEntity
    {
        public string Name { get; set; }

        public int DaysAllowed { get; set; }
        public bool IsPaidLeave { get; set; }
        
    }
}
