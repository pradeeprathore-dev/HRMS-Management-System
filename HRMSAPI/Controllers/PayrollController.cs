using Entites.Model;
using Helpers;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Services;
using HRMSAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly HRMSDbContext _context;
        private readonly INotificationService _notificationService;

        public PayrollController(
    HRMSDbContext context,
    INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        private async Task<int> GetWorkingDays(
    int month,
    int year)
        {
            int workingDays = 0;

            int totalDays =
                DateTime.DaysInMonth(year, month);

            // Company Holidays
            var holidays =
                await _context.Holidays
                .Where(x =>
                    x.Date.Month == month &&
                    x.Date.Year == year)
                .Select(x => x.Date.Date)
                .ToListAsync();

            for (int day = 1; day <= totalDays; day++)
            {
                var date =
                    new DateTime(year, month, day);

                // Weekend
                if (date.DayOfWeek == DayOfWeek.Saturday ||
                    date.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                // Company Holiday
                if (holidays.Contains(date.Date))
                {
                    continue;
                }

                workingDays++;
            }

            return workingDays;
        }

        // =========================
        // GENERATE PAYROLL
        // =========================

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> GeneratePayroll(
            CreatePayrollDto dto)
        {
            var employee =
                await _context.Employees
                .FindAsync(dto.EmployeeId);

            if (employee == null)
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Employee not found",
                        Data = null
                    });
            }
            // Duplicate Payroll Check
            if (await _context.Payrolls.AnyAsync(x =>
    x.EmployeeId == dto.EmployeeId &&
    x.Month == dto.Month &&
    x.Year == dto.Year))
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Payroll already generated for this month",
                        Data = null
                    });
            }
            decimal basicSalary = employee.Salary;

            decimal hra = basicSalary * 0.20m;

            decimal bonus = 0;
            // =====================================
            // WORKING DAYS
            // =====================================

            int workingDays =
                   await GetWorkingDays(
                    dto.Month,
                    dto.Year);

            // =====================================
            // PRESENT DAYS
            // =====================================

            int presentDays =
                await _context.Attendances
                .CountAsync(x =>
                    x.EmployeeId == dto.EmployeeId &&
                    x.Date.Month == dto.Month &&
                    x.Date.Year == dto.Year);

            // Example: December Bonus
            if (dto.Month == 12)
            {
                bonus = basicSalary * 0.10m;
            }
            // =====================================
            // PAID LEAVE DAYS
            // =====================================

            var paidLeaves =
                await _context.LeaveRequests
                .Include(x => x.LeaveType)
                .Where(x =>
                    x.EmployeeId == dto.EmployeeId &&
                    x.Status == "Approved" &&
                    x.LeaveType.IsPaidLeave &&
                    x.StartDate.Month == dto.Month &&
                    x.StartDate.Year == dto.Year)
                .ToListAsync();

            int paidLeaveDays = 0;

            foreach (var leave in paidLeaves)
            {
                paidLeaveDays +=
                    (leave.EndDate.Date -
                     leave.StartDate.Date).Days + 1;
            }
            // =====================================
            // UNPAID LEAVE CALCULATION //
            // =====================================

            var unpaidLeaves =
    await _context.LeaveRequests
    .Include(x => x.LeaveType)
                .Where(x =>
                    x.EmployeeId == dto.EmployeeId &&
                    x.Status == "Approved" &&
                    x.LeaveType.IsPaidLeave == false &&
                    x.StartDate.Month == dto.Month &&
                    x.StartDate.Year == dto.Year)
                .ToListAsync();

            int unpaidDays = 0;

            foreach (var leave in unpaidLeaves)
            {
                unpaidDays +=
                    (leave.EndDate.Date - leave.StartDate.Date).Days + 1;
            }
            // =====================================
            // ABSENT DAYS
            // =====================================

            int absentDays =
                workingDays -
                presentDays -
                paidLeaveDays;

            if (absentDays < 0)
            {
                absentDays = 0;
            }

            // =====================================
            // TOTAL DEDUCTION DAYS
            // =====================================

            int totalDeductionDays =
                unpaidDays +
                absentDays;

            decimal perDaySalary 
                = basicSalary / 30;

            //============================================================
            // Net salary 
            //============================================================

            decimal deduction =
                totalDeductionDays * perDaySalary;

            decimal netSalary =
    basicSalary +
    hra +
    bonus -
    deduction;


            var payroll =
    new Payroll
    {
        EmployeeId = dto.EmployeeId,

        Month = dto.Month,

        Year = dto.Year,

        BasicSalary = basicSalary,

        HRA = hra,

        Bonus = bonus,

        Deduction = deduction,

        NetSalary = netSalary,

        Status = "Draft",

        PaymentDate = null,

        TransactionId = null
    };
            _context.Payrolls.Add(payroll);

            await _context.SaveChangesAsync();
            await _notificationService.Create(
    new CreateNotificationDto
    {
        EmployeeId = payroll.EmployeeId,

        Title = "Payroll Generated",

        Message =
            $"Your payroll for {dto.Month}/{dto.Year} has been generated successfully."
    });

            return Ok(
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "Payroll generated successfully",
                    Data = new
                    {
                        payroll.Id,
                        payroll.EmployeeId,
                        payroll.Month,
                        payroll.Year,
                        payroll.BasicSalary,
                        payroll.HRA,
                        payroll.Bonus,
                        payroll.Deduction,
                        payroll.NetSalary,
                        payroll.Status
                    }

                });
        }

        // =========================
        // ALL PAYROLLS
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAllPayrolls()
        {
            var data =
                await _context.Payrolls
                .Include(x => x.Employee)
                .Select(x => new PayrollResponseDto
                {
                    Id = x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    Month = x.Month,

                    Year = x.Year,

                    BasicSalary = x.BasicSalary,

                    HRA = x.HRA,

                    Bonus = x.Bonus,

                    Deduction = x.Deduction,

                    NetSalary = x.NetSalary,
                    Status = x.Status,

                    PaymentDate = x.PaymentDate,

                    TransactionId = x.TransactionId
                })
                .ToListAsync();

            return Ok(
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "Payrolls fetched successfully",
                    Data = data
                });
        }

        // =========================
        // MY PAYROLLS
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("MyPayrolls/{employeeId}")]
        public async Task<IActionResult> MyPayrolls(
            int employeeId)
        {
            if (User.IsInRole("2"))
            {
                var employeeIdClaim =
                    User.FindFirst("EmployeeId")?.Value;

                if (!int.TryParse(employeeIdClaim, out employeeId))
                {
                    return Unauthorized(
                        new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Employee identity not found in token",
                            Data = null
                        });
                }
            }

            var data =
                await _context.Payrolls
                .Include(x => x.Employee)
                .Where(x =>
                    x.EmployeeId == employeeId)
                .Select(x => new PayrollResponseDto
                {
                    Id = x.Id,

                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

                    Month = x.Month,

                    Year = x.Year,

                    BasicSalary = x.BasicSalary,

                    HRA = x.HRA,

                    Bonus = x.Bonus,

                    Deduction = x.Deduction,

                    NetSalary = x.NetSalary,
                    Status = x.Status,

                    PaymentDate = x.PaymentDate,

                    TransactionId = x.TransactionId
                })
                .ToListAsync();

            return Ok(
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "My payrolls fetched successfully",
                    Data = data
                });
        }
        // =========================
        // APPROVE PAYROLL
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> ApprovePayroll(
            int id)
        {
            var payroll =
                await _context.Payrolls
                .FindAsync(id);

            if (payroll == null)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Payroll not found",
                        Data = null
                    });
            }

            if (payroll.Status == "Paid")
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Payroll already paid",
                        Data = null
                    });
            }

            payroll.Status = "Approved";

            await _context.SaveChangesAsync();

            return Ok(
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "Payroll approved successfully",
                   Data = payroll
                });
        }
        // =========================
        // MARK PAYROLL AS PAID
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("MarkPaid/{id}")]
        public async Task<IActionResult> MarkPaid(
            int id)
        {
            var payroll =
                await _context.Payrolls
                .FindAsync(id);

            if (payroll == null)
            {
                return NotFound(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Payroll not found",
                        Data = null
                    });
            }

            if (payroll.Status != "Approved")
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Payroll must be Approved before payment",
                        Data = null
                    });
            }

            payroll.Status = "Paid";

            payroll.PaymentDate =
                DateTime.Now;

            payroll.TransactionId =
                Guid.NewGuid()
                .ToString()
                .Substring(0, 12)
                .ToUpper();

            await _context.SaveChangesAsync();

            return Ok(
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "Payroll marked as Paid",
                    Data = payroll
                });
        }
    }
}