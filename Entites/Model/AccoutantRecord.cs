using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class AccountRecord : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }
}
