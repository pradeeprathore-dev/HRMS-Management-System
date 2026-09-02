using Entites.Model;
using Helpers;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly HRMSDbContext _context;

        public AttendanceController(HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // PUNCH IN
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpPost("PunchIn")]
        public async Task<IActionResult> PunchIn()
        {
            var employeeIdClaim =
    User.FindFirst("EmployeeId")?.Value;

            if (!int.TryParse(employeeIdClaim, out int employeeId))
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Employee identity not found in token",
                    Data = null
                });
            }
            var employee =
    await _context.Employees
    .Include(e => e.Shift)
    .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Employee not found",
                    Data = null
                });
            }
            if (employee.ShiftId == null || employee.Shift == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Employee is not assigned to any shift.",
                    Data = null
                });
            }

            var today =
                DateTime.Today;
            // =========================
            // WEEKEND CHECK
            // =========================

            if (today.DayOfWeek == DayOfWeek.Saturday ||
                today.DayOfWeek == DayOfWeek.Sunday)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Attendance cannot be marked on weekends.",
                    Data = null
                });
            }

            // =========================
            // HOLIDAY CHECK
            // =========================

            var isHoliday =
                await _context.Holidays
                .AnyAsync(x => x.Date.Date == today);

            if (isHoliday)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Today is a company holiday. Attendance cannot be marked.",
                    Data = null
                });
            }

            var alreadyPunched =
                await _context.Attendances
                .AnyAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.Date.Date == today);

            if (alreadyPunched)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Already punched in today",
                    Data = null
                });
            }
            var punchInTime = DateTime.Now;

            var shiftStart =
                today.Add(employee.Shift.StartTime);

            var lateTime =
                shiftStart.AddMinutes(employee.Shift.GraceMinutes);

            var status =
                punchInTime > lateTime
                    ? "Late"
                    : "Present";

            var attendance =
                new Attendance
                {
                    EmployeeId = employeeId,

                    Date = today,

                    PunchIn = punchInTime,

                    Status = status
                };

            _context.Attendances.Add(attendance);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Punch In Successful",
                Data = new
                {
                    attendance.Id,
                    attendance.EmployeeId,
                    attendance.Date,
                    attendance.PunchIn,
                    attendance.PunchOut,
                    attendance.Status
                }
            });
        }

        // =========================
        // PUNCH OUT
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpPut("PunchOut/{employeeId}")]
        public async Task<IActionResult> PunchOut(
    int employeeId)
        {
            // =========================
            // GET ROLE FROM JWT
            // =========================

            var roleClaim =
                User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            // =========================
            // NORMAL USER
            // =========================

            if (roleClaim == "2")
            {
                var employeeIdClaim =
                    User.FindFirst("EmployeeId")?.Value;

                if (!int.TryParse(employeeIdClaim, out int currentEmployeeId))
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Employee identity not found in token",
                        Data = null
                    });
                }

                // Ignore employeeId coming from URL
                employeeId = currentEmployeeId;
            }

            // =========================
            // TODAY
            // =========================

            var today =
                DateTime.Today;

            // =========================
            // FIND ATTENDANCE
            // =========================

            var attendance =
                await _context.Attendances
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employeeId &&
                    x.Date.Date == today);

            if (attendance == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Punch in not found",
                    Data = null
                });
            }

            // =========================
            // PUNCH OUT
            // =========================

            attendance.PunchOut = DateTime.Now;

            await _context.SaveChangesAsync();

            // =========================
            // WORKING MINUTES
            // =========================

            var workingMinutes =
                (int)(attendance.PunchOut.Value - attendance.PunchIn).TotalMinutes;

            // =========================
            // RESPONSE
            // =========================

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Punch Out Successful",
                Data = new
                {
                    attendance.Id,
                    attendance.EmployeeId,
                    attendance.Date,
                    attendance.PunchIn,
                    attendance.PunchOut,
                    attendance.Status,
                    WorkingMinutes = workingMinutes
                }
            });
        }

        // =========================
        // MY ATTENDANCE
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("MyAttendance/{employeeId}")]
        public async Task<IActionResult> MyAttendance(
    int employeeId)
        {
            // =========================
            // GET ROLE FROM JWT
            // =========================

            var roleClaim =
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.Role
                )?.Value;

            // =========================
            // NORMAL USER
            // =========================

            if (roleClaim == "2")
            {
                var employeeIdClaim =
                    User.FindFirst("EmployeeId")?.Value;

                if (!int.TryParse(
                        employeeIdClaim,
                        out int currentEmployeeId))
                {
                    return Unauthorized(
                        new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Employee identity not found in token",
                            Data = null
                        });
                }

                // Ignore employeeId coming from URL
                employeeId = currentEmployeeId;
            }

            // =========================
            // GET ATTENDANCE
            // =========================

            var data =
                await _context.Attendances
                .Include(x => x.Employee)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new
                {
                    x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    x.Date,

                    x.PunchIn,

                    x.PunchOut,

                    x.Status,

                    WorkingMinutes =
                        x.PunchOut.HasValue
                            ? (int)(
                                x.PunchOut.Value -
                                x.PunchIn
                            ).TotalMinutes
                            : 0
                })
                .ToListAsync();

            return Ok(
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "Attendance fetched successfully",
                    Data = data
                });
        }
        // =========================
        // ALL ATTENDANCE
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAllAttendance()
        {
            var data =
                await _context.Attendances
                .Include(x => x.Employee)
                .Select(x => new
                {
                    x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    x.Date,

                    x.PunchIn,

                    x.PunchOut,

                    x.Status,

                    WorkingMinutes =
    x.PunchOut.HasValue
        ? (int)(x.PunchOut.Value - x.PunchIn).TotalMinutes
        : 0
                })
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Attendance fetched successfully",
                Data = data
            });
        }
    }
}