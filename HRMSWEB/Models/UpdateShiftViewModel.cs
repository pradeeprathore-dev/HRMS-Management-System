using System.ComponentModel.DataAnnotations;

namespace HRMSWEB.Models
{

    public class UpdateShiftViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ShiftName { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int GraceMinutes { get; set; }

        [Required]
        public TimeSpan HalfDayTime { get; set; }

        public bool IsActive { get; set; }
    }
}