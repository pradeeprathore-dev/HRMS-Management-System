namespace HRMSWEB.Models
{
    public class PayrollViewModel
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

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

        public string Status { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? TransactionId { get; set; }
    }
}