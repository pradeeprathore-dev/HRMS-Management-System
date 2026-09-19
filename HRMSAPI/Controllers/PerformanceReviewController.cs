using Helpers;
using HRMSAPI.DTOs;
using HRMSAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceReviewController : ControllerBase
    {
        private readonly IPerformanceReviewService _service;

        public PerformanceReviewController(
            IPerformanceReviewService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL REVIEWS
        // =========================

        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Performance reviews fetched successfully",
                Data = data
            });
        }

        // =========================
        // GET REVIEW BY ID
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
                    Message = "Performance review not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Performance review fetched successfully",
                Data = data
            });
        }

        // =========================
        // MY PERFORMANCE
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("Employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
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
                await _service.GetByEmployeeId(employeeId);

            return Ok(
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "Employee performance fetched successfully",
                    Data = data
                });
        }

        // =========================
        // CREATE REVIEW
        // =========================

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePerformanceReviewDto dto)
        {
            var data = await _service.Create(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Performance review created successfully",
                Data = data
            });
        }

        // =========================
        // UPDATE REVIEW
        // =========================

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdatePerformanceReviewDto dto)
        {
            var result =
                await _service.Update(id, dto);

            if (!result)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Performance review not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Performance review updated successfully",
                Data = null
            });
        }

        // =========================
        // DELETE REVIEW
        // =========================

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _service.Delete(id);

            if (!result)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Performance review not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Performance review deleted successfully",
                Data = null
            });
        }
    }
}