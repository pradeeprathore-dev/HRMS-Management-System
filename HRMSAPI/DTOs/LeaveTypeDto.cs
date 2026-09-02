namespace HRMSAPI.DTOs
{
    public class LeaveTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DaysAllowed { get; set; }

        public bool IsPaidLeave { get; set; }

        public int UsedDays { get; set; }

        public int RemainingDays { get; set; }
    }
}
