namespace HRMSWEB.Models
{
    public class PerformanceReviewViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int Rating { get; set; }

        public string Reviewer { get; set; }

        public string Comments { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool PromotionRecommended { get; set; }
    }
}