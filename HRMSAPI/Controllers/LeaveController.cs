using Entites.Model;
using Helpers;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Helpers;
using HRMSAPI.Services;
using HRMSAPI.Services.Interfaces;
using HRMSWEB.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly HRMSDbContext _context;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;

        public LeaveController(
     HRMSDbContext context,
     IEmailService emailService,
     INotificationService notificationService)
        {
            _context = context;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        // =========================
        // APPLY LEAVE
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpPost("Apply")]
        public async Task<IActionResult> ApplyLeave(
     CreateLeaveRequestDto dto)
        {
            // ==========================================
            // GET EMPLOYEE ID FROM JWT FOR NORMAL USER
            // ==========================================

            int employeeId;

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
            else
            {
                // Admin keeps existing behavior
                employeeId = dto.EmployeeId;
            }

            // ==========================================
            // EMPLOYEE CHECK
            // ==========================================

            var employee =
                await _context.Employees
                .FindAsync(employeeId);

            if (employee == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Employee not found",
                    Data = null
                });
            }

            // ==========================================
            // LEAVE TYPE CHECK
            // ==========================================

            var leaveType =
                await _context.LeaveTypes
                .FindAsync(dto.LeaveTypeId);

            if (leaveType == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Leave Type not found",
                    Data = null
                });
            }

            // ==========================================
            // DATE VALIDATION
            // ==========================================

            if (dto.EndDate < dto.StartDate)
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "End Date cannot be less than Start Date",
                        Data = null
                    });
            }

            // ==========================================
            // LEAVE BALANCE VALIDATION
            // ==========================================

            int requestedDays =
                (dto.EndDate.Date -
                 dto.StartDate.Date).Days + 1;

            var approvedLeaves =
                await _context.LeaveRequests
                .Where(x =>
                    x.EmployeeId == employeeId &&
                    x.LeaveTypeId == dto.LeaveTypeId &&
                    x.Status == "Approved")
                .ToListAsync();

            int usedDays = 0;

            foreach (var leave in approvedLeaves)
            {
                if (leave.EndDate < leave.StartDate)
                {
                    continue;
                }

                var days =
                    (leave.EndDate.Date -
                     leave.StartDate.Date).Days + 1;

                if (days > 365)
                {
                    continue;
                }

                usedDays += days;
            }

            // Paid leave ke liye hi balance check hoga
            if (leaveType.IsPaidLeave)
            {
                int remainingDays =
                    leaveType.DaysAllowed - usedDays;

                if (requestedDays > remainingDays)
                {
                    return BadRequest(
                        new ApiResponse<object>
                        {
                            Success = false,
                            Message =
                                $"Only {remainingDays} leave days remaining",
                            Data = null
                        });
                }
            }

            // ==========================================
            // CREATE LEAVE REQUEST
            // ==========================================

            var leaveRequest =
                new LeaveRequest
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = dto.LeaveTypeId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Reason = dto.Reason,
                    Status = "Pending"
                };

            _context.LeaveRequests.Add(leaveRequest);

            await _context.SaveChangesAsync();

            // ==========================================
            // NOTIFICATION
            // ==========================================

            await _notificationService.Create(
                new CreateNotificationDto
                {
                    EmployeeId = employeeId,

                    Title = "Leave Applied",

                    Message =
                        $"Your leave request from {dto.StartDate:d} to {dto.EndDate:d} has been submitted."
                });

            // ==========================================
            // EMAIL
            // ==========================================

            await _emailService.SendEmailAsync(
                employee.Email,
                "Leave Applied",
                $"Your leave request from {dto.StartDate:d} to {dto.EndDate:d} has been submitted successfully.");

            // ==========================================
            // RESPONSE
            // ==========================================

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Leave Applied Successfully",
                Data = new
                {
                    leaveRequest.Id,
                    leaveRequest.EmployeeId,
                    leaveRequest.LeaveTypeId,
                    leaveRequest.StartDate,
                    leaveRequest.EndDate,
                    leaveRequest.Reason,
                    leaveRequest.Status
                }
            });
        }

        // =========================
        // GET ALL LEAVES
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeaves()
        {
            var data =
                await _context.LeaveRequests
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .Select(x => new LeaveRequestResponseDto
                {
                    Id = x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    LeaveType = x.LeaveType.Name,

                    StartDate = x.StartDate,

                    EndDate = x.EndDate,

                    Reason = x.Reason,

                    Status = x.Status
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Leaves fetched successfully",
                Data = data
            });
        }

        // =========================
        // MY LEAVES
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("MyLeaves/{employeeId}")]
        public async Task<IActionResult> MyLeaves(
    int employeeId)
        {
            // ==========================================
            // NORMAL EMPLOYEE → JWT EMPLOYEE ID
            // ==========================================

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

            // ==========================================
            // ADMIN → EXISTING employeeId FROM ROUTE
            // ==========================================

            var data =
                await _context.LeaveRequests
                .Include(x => x.Employee)
                .Include(x => x.LeaveType)
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new LeaveRequestResponseDto
                {
                    Id = x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    LeaveType = x.LeaveType.Name,

                    StartDate = x.StartDate,

                    EndDate = x.EndDate,

                    Reason = x.Reason,

                    Status = x.Status
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "My leaves fetched successfully",
                Data = data
            });
        }

        // =========================
        // APPROVE LEAVE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> ApproveLeave(int id)
        {
            var leave =
    await _context.LeaveRequests
    .Include(x => x.Employee)
    .FirstOrDefaultAsync(x => x.Id == id);

            if (leave == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Leave request not found",
                    Data = null
                });
            }

            leave.Status = "Approved";

            await _context.SaveChangesAsync();
            await _notificationService.Create(
    new CreateNotificationDto
    {
        EmployeeId = leave.EmployeeId,

        Title = "Leave Approved",

        Message =
            $"Your leave request from {leave.StartDate:d} to {leave.EndDate:d} has been approved."
    });
            await _emailService.SendEmailAsync(
    leave.Employee.Email,
    "Leave Approved",
    $"Your leave request from {leave.StartDate:d} to {leave.EndDate:d} has been approved.");

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Leave Approved Successfully",
                Data = null
            });
        }

        // =========================
        // REJECT LEAVE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> RejectLeave(int id)
        {
            var leave =
    await _context.LeaveRequests
    .Include(x => x.Employee)
    .FirstOrDefaultAsync(x => x.Id == id);

            if (leave == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Leave request not found",
                    Data = null
                });
            }

            leave.Status = "Rejected";

            await _context.SaveChangesAsync();
            await _notificationService.Create(
    new CreateNotificationDto
    {
        EmployeeId = leave.EmployeeId,

        Title = "Leave Rejected",

        Message =
            $"Your leave request from {leave.StartDate:d} to {leave.EndDate:d} has been rejected."
    });
            await _emailService.SendEmailAsync(
    leave.Employee.Email,
    "Leave Rejected",
    $"Your leave request from {leave.StartDate:d} to {leave.EndDate:d} has been rejected.");

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Leave Rejected Successfully",
                Data = null
            });
        }
    }
}