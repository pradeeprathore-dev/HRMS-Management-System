using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly HRMSDbContext _context;

        public LeaveTypeController(
            HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL LEAVE TYPES
        // =========================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data =
                await _context.LeaveTypes
                .Select(x => new LeaveTypeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DaysAllowed = x.DaysAllowed,
                    IsPaidLeave = x.IsPaidLeave
                })
                .ToListAsync();

            return Ok(data);
        }

        [Authorize(Roles = "1,2")]
        [HttpGet("Balance/{employeeId}")]
        public async Task<IActionResult> GetLeaveBalance(
    int employeeId)
        {
            // =========================
            // EMPLOYEE ID SECURITY
            // =========================

            if (User.IsInRole("2"))
            {
                var employeeIdClaim =
                    User.FindFirst("EmployeeId")?.Value;

                if (!int.TryParse(employeeIdClaim, out employeeId))
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Employee identity not found in token",
                        Data = null
                    });
                }
            }

            var leaveTypes =
                await _context.LeaveTypes.ToListAsync();

            var result =
                new List<LeaveTypeDto>();

            foreach (var leaveType in leaveTypes)
            {
                var approvedLeaves =
                    await _context.LeaveRequests
                    .Where(x =>
                        x.EmployeeId == employeeId &&
                        x.LeaveTypeId == leaveType.Id &&
                        x.Status == "Approved")
                    .ToListAsync();

                int usedDays = 0;

                foreach (var leave in approvedLeaves)
                {
                    usedDays +=
                        (leave.EndDate.Date -
                         leave.StartDate.Date).Days + 1;
                }

                result.Add(
                    new LeaveTypeDto
                    {
                        Id = leaveType.Id,
                        Name = leaveType.Name,
                        DaysAllowed = leaveType.DaysAllowed,
                        UsedDays = usedDays,
                        RemainingDays =
                            leaveType.DaysAllowed - usedDays
                    });
            }

            return Ok(result);
        }
    }
}