using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Shift: BaseEntity
    {
        public string ShiftName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        // Grace minutes
        public int GraceMinutes { get; set; }

        // Half Day Cutoff
        public TimeSpan HalfDayTime { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
