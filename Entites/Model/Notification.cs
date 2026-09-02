using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Notification : BaseEntity
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;
    }
}
