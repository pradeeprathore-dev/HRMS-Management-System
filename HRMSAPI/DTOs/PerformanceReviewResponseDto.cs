namespace HRMSAPI.DTOs
{
    public class PerformanceReviewResponseDto
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public int Rating { get; set; }

        public string Reviewer { get; set; }

        public string Comments { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool PromotionRecommended { get; set; }
    }
}