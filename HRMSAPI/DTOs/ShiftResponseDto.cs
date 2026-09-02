namespace HRMSAPI.DTOs
{
    public class ShiftResponseDto
    {
        public int Id { get; set; }

        public string ShiftName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int GraceMinutes { get; set; }

        public TimeSpan HalfDayTime { get; set; }

        public bool IsActive { get; set; }
    }
}