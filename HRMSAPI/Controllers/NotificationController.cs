using Helpers;
using HRMSAPI.DTOs;
using HRMSAPI.Services;
using HRMSAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(
            INotificationService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Notifications fetched successfully",
                Data = data
            });
        }

        // =========================
        // GET BY ID
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetById(id);

            if (data == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Notification not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Notification fetched successfully",
                Data = data
            });
        }

        // =========================
        // MY NOTIFICATIONS
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("Employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(
            int employeeId)
        {
            var data =
                await _service.GetByEmployeeId(employeeId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Notifications fetched successfully",
                Data = data
            });
        }

        // =========================
        // CREATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateNotificationDto dto)
        {
            var data =
                await _service.Create(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Notification created successfully",
                Data = data
            });
        }

        // =========================
        // MARK AS READ
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpPut("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkAsRead(
            int id)
        {
            var result =
                await _service.MarkAsRead(id);

            if (!result)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Notification not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Notification marked as read",
                Data = null
            });
        }

        // =========================
        // DELETE
        // =========================

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var result =
                await _service.Delete(id);

            if (!result)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Notification not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Notification deleted successfully",
                Data = null
            });
        }
        // =========================
        // UNREAD COUNT
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("UnreadCount/{employeeId}")]
        public async Task<IActionResult> UnreadCount(
            int employeeId)
        {
            var data =
                await _service.GetUnreadCount(employeeId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Unread notification count fetched successfully",
                Data = data
            });
        }
        // =========================
        // LATEST 5 NOTIFICATIONS
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("Latest/{employeeId}")]
        public async Task<IActionResult> Latest(
            int employeeId)
        {
            var data =
                await _service.GetLatest(employeeId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Latest notifications fetched successfully",
                Data = data
            });
        }
        // =========================
        // BELL DATA
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("Bell/{employeeId}")]
        public async Task<IActionResult> Bell(
            int employeeId)
        {
            var data =
                await _service.GetBellData(employeeId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Bell data fetched successfully",
                Data = data
            });
        }
    }
}