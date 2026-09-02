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
    public class DashboardController : ControllerBase
    {
        private readonly HRMSDbContext _context;

        public DashboardController(
            HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // ADMIN DASHBOARD
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dto =
                new DashboardDto
                {
                    TotalEmployees =
                        await _context.Employees.CountAsync(),

                    TotalDepartments =
                        await _context.Departments.CountAsync(),

                    PendingLeaves =
                        await _context.LeaveRequests
                        .CountAsync(x =>
                            x.Status == "Pending"),

                    PresentToday =
    await _context.Attendances
    .CountAsync(x =>
        x.Date.Date == DateTime.Today &&
        x.Status == "Present"),
                    LateToday =
    await _context.Attendances
    .CountAsync(x =>
        x.Date.Date == DateTime.Today &&
        x.Status == "Late"),
                    AbsentToday =
                     (
                       await _context.Employees.CountAsync()
                      )
-
                     (
                      await _context.Attendances
    .Where(x => x.Date.Date == DateTime.Today)
    .Select(x => x.EmployeeId)
    .Distinct()
    .CountAsync()
),
                    PendingPayrolls =
    await _context.Payrolls
    .CountAsync(x =>
        x.Status == "Draft"),
                    TodayHoliday =
    await _context.Holidays
    .Where(x =>
        x.Date.Date == DateTime.Today)
    .Select(x => x.Name)
    .FirstOrDefaultAsync()
    ?? "No Holiday"
                };

            return Ok(dto);
        }


        // =========================
        // LEAVE CHART
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet("LeaveChart")]
        public async Task<IActionResult> LeaveChart()
        {
            var pending =
                await _context.LeaveRequests
                .CountAsync(x =>
                    x.Status == "Pending");

            var approved =
                await _context.LeaveRequests
                .CountAsync(x =>
                    x.Status == "Approved");

            var rejected =
                await _context.LeaveRequests
                .CountAsync(x =>
                    x.Status == "Rejected");

            

            return Ok(new
            {
                Pending = pending,
                Approved = approved,
                Rejected = rejected
            });

        }
        [Authorize(Roles = "1")]
        [HttpGet("AttendanceChart")]
        public async Task<IActionResult> AttendanceChart()
        {
            var present =
                await _context.Attendances
                .CountAsync(x =>
                    x.Status == "Present");

            var late =
                await _context.Attendances
                .CountAsync(x =>
                    x.Status == "Late");

            var totalEmployees =
                await _context.Employees
                .CountAsync();

            var todayAttendance =
                await _context.Attendances
                .Where(x =>
                    x.Date.Date == DateTime.Today)
                .Select(x => x.EmployeeId)
                .Distinct()
                .CountAsync();

            var absent =
                totalEmployees - todayAttendance;

            return Ok(new
            {
                Present = present,
                Late = late,
                Absent = absent
            });
        }
        [Authorize(Roles = "1")]
        [HttpGet("RecentLeaves")]
        public async Task<IActionResult> GetRecentLeaves()
        {
            var recentLeaves = await _context.LeaveRequests
                .AsNoTracking()
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new RecentLeaveDto
                {
                    LeaveRequestId = x.Id,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee.FirstName + " " + x.Employee.LastName,
                    LeaveType = x.LeaveType.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return Ok(recentLeaves);
        }
        [Authorize(Roles = "1")]
        [HttpGet("RecentEmployees")]
        public async Task<IActionResult> GetRecentEmployees()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Include(x => x.Department)
                .Include(x => x.Designation)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new RecentEmployeeDto
                {
                    EmployeeId = x.Id,

                    FullName = x.FirstName + " " + x.LastName,

                    Department = x.Department.Name,

                    Designation = x.Designation.Title,

                    ProfileImage = x.ProfileImage,

                    JoinedOn = x.CreatedAt
                })
                .ToListAsync();

            return Ok(employees);
        }
        [Authorize(Roles = "1")]
        [HttpGet("UpcomingHolidays")]
        public async Task<IActionResult> GetUpcomingHolidays()
        {
            var today = DateTime.Today;

            var holidays = await _context.Holidays
                .AsNoTracking()
                .Where(x => x.Date >= today)
                .OrderBy(x => x.Date)
                .Take(5)
                .Select(x => new UpcomingHolidayDto
                {
                    HolidayId = x.Id,
                    Name = x.Name,
                    Date = x.Date,
                    Description = x.Description,
                    IsOptional = x.IsOptional
                })
                .ToListAsync();

            return Ok(holidays);
        }
    }
}