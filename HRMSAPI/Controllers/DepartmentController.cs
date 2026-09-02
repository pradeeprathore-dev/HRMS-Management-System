using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly HRMSDbContext _context;

        public DepartmentController(HRMSDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Departments
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();

            return Ok(data);
        }

        // =========================
        // GET BY ID
        // =========================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dept = await _context.Departments
                .Where(d => d.Id == id)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .FirstOrDefaultAsync();

            if (dept == null)
                return NotFound();

            return Ok(dept);
        }

        // =========================
        // CREATE
        // =========================

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateDepartmentDto dto)
        {
            var dept = new Department
            {
                Name = dto.Name
            };
            if (await _context.Departments
    .AnyAsync(x => x.Name == dto.Name))
            {
                return BadRequest(
                    "Department already exists");
            }

            _context.Departments.Add(dept);

            await _context.SaveChangesAsync();

            return Ok(dept);
        }

        // =========================
        // UPDATE
        // =========================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateDepartmentDto dto)
        {
            var dept =
                await _context.Departments.FindAsync(id);

            if (dept == null)
                return NotFound();

            dept.Name = dto.Name;

            await _context.SaveChangesAsync();

            return Ok(dept);
        }

        // =========================
        // DELETE
        // =========================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dept =
                await _context.Departments.FindAsync(id);

            if (dept == null)
                return NotFound();
            var hasEmployees =
    await _context.Employees
    .AnyAsync(x => x.DepartmentId == id);

            if (hasEmployees)
            {
                return BadRequest(
                    "Department is assigned to employees");
            }

            _context.Departments.Remove(dept);

            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}