using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Models
{
    public class CreatePerformanceReviewViewModel
    {
        public int EmployeeId { get; set; }

        public int Rating { get; set; }

        public string Reviewer { get; set; }

        public string Comments { get; set; }

        public DateTime ReviewDate { get; set; }
            = DateTime.Now;

        public bool PromotionRecommended { get; set; }

        public List<SelectListItem> Employees
        {
            get;
            set;
        }
        =
        new();
    }
}