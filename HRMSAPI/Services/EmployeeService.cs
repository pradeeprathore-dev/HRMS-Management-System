using AutoMapper;
using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        private readonly IMapper _mapper;

        private readonly HRMSDbContext _context;

        public EmployeeService(
            IEmployeeRepository repository,
            IMapper mapper,
            HRMSDbContext context)
        {
            _repository = repository;

            _mapper = mapper;

            _context = context;
        }

        // ✅ GET ALL

        public async Task<List<EmployeeResponseDto>> GetAll(
            PaginationDto paginationDto)
        {
            var employees =
                await _repository.GetAll(
                    paginationDto);

            return employees.Select(emp =>
    new EmployeeResponseDto
    {
        Id = emp.Id,
        FirstName = emp.FirstName,
        LastName = emp.LastName,
        Email = emp.Email,

        Department = emp.Department?.Name,

        DepartmentId = emp.DepartmentId,

        Designation = emp.Designation?.Title,

        DesignationId = emp.DesignationId,

        Salary = emp.Salary,

        ProfileImage = emp.ProfileImage,

        ShiftId = emp.ShiftId,

        ShiftName = emp.Shift?.ShiftName
    }).ToList();
        }

        // ✅ GET BY ID

        public async Task<EmployeeResponseDto?> GetById(
            int id)
        {
            var employee =
                await _repository.GetById(id);

            if (employee == null)
                return null;

            return new EmployeeResponseDto
            {
                Id = employee.Id,

                FirstName = employee.FirstName,

                LastName = employee.LastName,

                Email = employee.Email,

                Department =
    employee.Department?.Name,

                DepartmentId =
    employee.DepartmentId,

                Designation =
    employee.Designation?.Title,

                DesignationId =
    employee.DesignationId,

                Salary = employee.Salary,

                ProfileImage =
        employee.ProfileImage,

                ShiftId =
        employee.ShiftId,

                ShiftName =
        employee.Shift?.ShiftName
            };
        }

        // ✅ CREATE

        public async Task<EmployeeResponseDto> Create(
            CreateEmployeeDto dto)
        {
            var existingEmployee =
                await _repository.GetByEmail(
                    dto.Email);

            if (existingEmployee != null)
            {
                throw new Exception(
                    "Email already exists");
            }

            // ✅ Department Validation

            var departmentExists =
                await _context.Departments
                .AnyAsync(x =>
                    x.Id == dto.DepartmentId);

            if (!departmentExists)
                throw new Exception(
                    "Department not found");

            // ✅ Designation Validation

            var designationExists =
                await _context.Designations
                .AnyAsync(x =>
                    x.Id == dto.DesignationId);

            if (!designationExists)
                throw new Exception(
                    "Designation not found");

            // =========================
            // Shift Validation
            // =========================

            if (dto.ShiftId.HasValue)
            {
                var shiftExists =
                    await _context.Shifts
                    .AnyAsync(x =>
                        x.Id == dto.ShiftId.Value &&
                        x.IsActive);

                if (!shiftExists)
                    throw new Exception(
                        "Shift not found or inactive");
            }

            var employee =
                _mapper.Map<Employee>(dto);

            var result =
                await _repository.Create(
                    employee);

            var createdEmployee =
    await _repository.GetById(result.Id);


            return new EmployeeResponseDto
            {
                Id = result.Id,

                FirstName = result.FirstName,

                LastName = result.LastName,

                Email = result.Email,

                Department =
                    result.Department?.Name,

                Designation =
                    result.Designation?.Title,

                Salary = result.Salary,

                ProfileImage =
                    result.ProfileImage
            };
        }

        // ✅ UPDATE

        public async Task<bool> Update(
    int id,
    UpdateEmployeeDto dto)
        {
            var emp =
                await _repository.GetById(id);

            if (emp == null)
                return false;

            // =========================
            // SHIFT VALIDATION
            // =========================

            if (dto.ShiftId.HasValue)
            {
                var shiftExists =
                    await _context.Shifts
                    .AnyAsync(x =>
                        x.Id == dto.ShiftId.Value &&
                        x.IsActive);

                if (!shiftExists)
                    throw new Exception(
                        "Shift not found or inactive");
            }

            _mapper.Map(dto, emp);

            await _repository.Update(emp);

            return true;
        }

        // ✅ DELETE

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        public async Task<List<EmployeeDropdownDto>> GetAllForDropdown()
        {
            var employees = await _repository.GetAllForDropdown();

            return employees.Select(e => new EmployeeDropdownDto
            {
                Id = e.Id,
                Name = e.FirstName + " " + e.LastName
            }).ToList();
        }
    }
}