using Entites.Model;
using HRMSAPI.Model;

namespace HRMSAPI.Data
{
    public class Salary : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        // Salary Structure
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal Allowances { get; set; }

        // Deductions
        public decimal Tax { get; set; }
        public decimal PF { get; set; }
        public decimal OtherDeductions { get; set; }

        // Final Salary
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }

        // Month Tracking
        public DateTime SalaryMonth { get; set; }
    }
}