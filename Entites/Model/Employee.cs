using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int DesignationId { get; set; }
        public Designation Designation { get; set; }

        public decimal Salary { get; set; }
        public string ProfileImage { get; set; }

        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
        public ICollection<ITAsset> ITAssets { get; set; }
        public ICollection<AccountRecord> AccountRecords { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<CanteenOrder> CanteenOrders { get; set; }
        public ICollection<PerformanceReview> PerformanceReviews
        {
            get;
            set;
        }
        =
        new List<PerformanceReview>();
        public int? ShiftId { get; set; }

        public Shift Shift { get; set; }

    }
}