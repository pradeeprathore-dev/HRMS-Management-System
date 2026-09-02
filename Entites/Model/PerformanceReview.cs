using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class PerformanceReview:BaseEntity
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int Rating { get; set; }

        public string Reviewer { get; set; }

        public string Comments { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool PromotionRecommended { get; set; }
    }
}
