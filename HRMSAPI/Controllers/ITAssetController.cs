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
    public class ITAssetController : ControllerBase
    {
        private readonly IITAssetService _service;

        public ITAssetController(IITAssetService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationDto paginationDto)
        {
            var data = await _service.GetAll(paginationDto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "IT Assets fetched successfully",
                Data = data
            });
        }

        // =========================
        // GET BY ID
        // =========================

        [Authorize(Roles = "1,2")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var asset = await _service.GetById(id);

            if (asset == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "IT Asset not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "IT Asset fetched successfully",
                Data = asset
            });
        }

        // =========================
        // CREATE
        // =========================

        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateITAssetDto dto)
        {
            var result = await _service.Create(dto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "IT Asset created successfully",
                Data = result
            });
        }
            //}
            //[Authorize(Roles = "1")]
            //[HttpPost]
            //public IActionResult Create(CreateITAssetDto dto)
            //{
            //    return Ok(dto);
            //}

            // =========================
            // UPDATE
            // =========================

        [Authorize(Roles = "1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateITAssetDto dto)
        {
            var updated = await _service.Update(id, dto);

            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "IT Asset not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "IT Asset updated successfully",
                Data = null
            });
        }

        // =========================
        // DELETE
        // =========================

        [Authorize(Roles = "1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.Delete(id);

            if (!deleted)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "IT Asset not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "IT Asset deleted successfully",
                Data = null
            });
        }
        [Authorize(Roles = "2")]
        [HttpGet("MyAssets")]
        public async Task<IActionResult> GetMyAssets(
    [FromQuery] PaginationDto paginationDto)
        {
            var employeeIdClaim = User.FindFirst("EmployeeId");

            if (employeeIdClaim == null)
            {
                return Unauthorized();
            }

            int employeeId = Convert.ToInt32(employeeIdClaim.Value);

            var data = await _service.GetMyAssets(employeeId, paginationDto);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "My IT Assets",
                Data = data
            });
        }
    }
}