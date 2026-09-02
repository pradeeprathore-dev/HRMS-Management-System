using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Payroll
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal HRA { get; set; }

        public decimal Bonus { get; set; }

        public decimal Deduction { get; set; }

        public decimal NetSalary { get; set; }

        // =========================
        // NEW FIELDS
        // =========================

        public string Status { get; set; } = "Draft";

        public DateTime? PaymentDate { get; set; }

        public string? TransactionId { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;
    }
}