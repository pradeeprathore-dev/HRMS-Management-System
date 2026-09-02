using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class EmployeeOnboarding : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime JoiningDate { get; set; }

        public string OfficialEmail { get; set; }

        public string SeatNumber { get; set; }

        public bool CredentialsCreated { get; set; }

        public bool DocumentsVerified { get; set; }

        public bool LaptopAllocated { get; set; }

        public bool ManagerAssigned { get; set; }

        public bool ProjectAssigned { get; set; }

        public bool IdCardGenerated { get; set; }

        public bool IsCompleted { get; set; }
    }
}