using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites
{
    public class Base
    {
        // Abstract base class — Abstraction
        public abstract class BaseEntity
        {
            public int Id { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsActive { get; set; }
            public abstract string GetDetails(); // abstract method
        }

        // Encapsulation — private fields with properties
        public abstract class BasePerson : BaseEntity
        {
            private string _email;
            public string Email
            {
                get => _email;
                set => _email = value?.Trim().ToLower()
                                ?? throw new ArgumentNullException();
            }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName => $"{FirstName} {LastName}";
        }

        // Inheritance — Company inherits BaseEntity
        public class Company : BaseEntity
        {
            public string CompanyName { get; set; }
            public string GST { get; set; }
            public string PAN { get; set; }
            public override string GetDetails() => $"{CompanyName} - {GST}";
        }

        // Inheritance — Employee inherits BasePerson
        public class Employee : BasePerson
        {
            public string EmployeeCode { get; set; }
            public int CompanyId { get; set; }
            public int DepartmentId { get; set; }
            public int ManagerId { get; set; }
            public override string GetDetails() => $"{FullName} [{EmployeeCode}]";
        }

        // Onboarding entity
        public class EmployeeOnboarding : BaseEntity
        {
            public int EmployeeId { get; set; }
            public bool IsCredentialCreated { get; set; }
            public bool IsInvitationSent { get; set; }
            public bool IsDocumentUploaded { get; set; }
            public bool IsSystemAllocated { get; set; }
            public bool IsIdCardGenerated { get; set; }
            public bool IsSittingAllocated { get; set; }
            public bool IsProjectAllocated { get; set; }
            public bool IsManagerAssigned { get; set; }

            public override string GetDetails()
            {
                throw new NotImplementedException();
            }
        }
    }
}
