namespace HRMSAPI.DTOs
{
    public class CreatePerformanceReviewDto
    {
        public int EmployeeId { get; set; }

        public int Rating { get; set; }

        public string Reviewer { get; set; }

        public string Comments { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool PromotionRecommended { get; set; }
    }
}