using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    internal interface IOnboardingService
    {
        // Interface — Polymorphism contract
        public interface IOnboardingService
        {
            Task CreateCredentialsAsync(int employeeId);
            Task SendInvitationMailAsync(int employeeId);
            Task UploadDocumentAsync(int employeeId, IFormFile file);
            Task AllocateSystemAsync(int employeeId, int systemId);
            Task GenerateIdCardAsync(int employeeId);
            Task AllocateSittingAsync(int employeeId, int seatId);
            Task AllocateProjectAsync(int employeeId, int projectId);
            Task AssignManagerAsync(int employeeId, int managerId);
        }

        public interface ICompanyService
        {
            Task<Company> RegisterCompanyAsync(Company company);
            Task<Company> GetByIdAsync(int id);
        }

        public interface IEmployeeService
        {
            Task<Employee> RegisterEmployeeAsync(Employee employee);
            Task<List<Employee>> GetAllAsync(int companyId);
        }
    }

    public class Employee
    {
    }

    public class Company
    {
    }
}
