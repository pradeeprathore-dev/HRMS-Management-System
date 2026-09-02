using System.ComponentModel.DataAnnotations;

namespace HRMSWEB.Models
{
    public class CreateShiftViewModel
    {
        [Required]
        public string ShiftName { get; set; } = string.Empty;

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int GraceMinutes { get; set; }

        [Required]
        public TimeSpan HalfDayTime { get; set; }

        public bool IsActive { get; set; } = true;
    }
}