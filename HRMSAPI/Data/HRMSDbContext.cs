using Entites.Model;
using HRMSAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HRMSAPI.Data
{
    public class HRMSDbContext : DbContext
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options)
            : base(options)
        {
        }

        // =========================
        // DbSets
        // =========================

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDetails> EmployeeDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }

        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<HRActivity> HRActivities { get; set; }

        public DbSet<Salary> Salaries { get; set; }
        public DbSet<AccountRecord> AccountRecords { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }

        public DbSet<ITAsset> ITAssets { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }

        public DbSet<AdminTask> AdminTasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<CanteenOrder> CanteenOrders { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public DbSet<TransportVehicle> TransportVehicles { get; set; }
        public DbSet<TransportRoute> TransportRoutes { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<EmployeeOnboarding> EmployeeOnboardings { get; set; }
        public DbSet<EmployeeDocument>
EmployeeDocuments
        { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        // =========================
        // Model Builder
        // =========================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Employee Relations
            // =========================
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Designation)
                .WithMany()
                .HasForeignKey(e => e.DesignationId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            // =========================
            // EmployeeDetails (1-1)
            // =========================
            modelBuilder.Entity<EmployeeDetails>()
                .HasOne(ed => ed.Employee)
                .WithOne()
                .HasForeignKey<EmployeeDetails>(ed => ed.EmployeeId);

            // =========================
            // Leave
            // =========================
            modelBuilder.Entity<Leave>()
                .HasOne(l => l.Employee)
                .WithMany(e => e.Leaves)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // Attendance
            // =========================
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId);

            // =========================
            // Salary
            // =========================
            modelBuilder.Entity<Salary>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId);

            // Decimal Fix
            modelBuilder.Entity<Salary>().Property(s => s.BasicSalary).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(s => s.HRA).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(s => s.Allowances).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(s => s.Tax).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(s => s.PF).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(s => s.OtherDeductions).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(s => s.GrossSalary).HasPrecision(18, 2);
            modelBuilder.Entity<Salary>().Property(s => s.NetSalary).HasPrecision(18, 2);

            // =========================
            // AccountRecord
            // =========================
            modelBuilder.Entity<AccountRecord>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.AccountRecords)
                .HasForeignKey(a => a.EmployeeId);

            modelBuilder.Entity<AccountRecord>()
                .Property(a => a.Amount)
                .HasPrecision(18, 2);

            // =========================
            // Project Mapping
            // =========================
            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeId);

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.ProjectId);

            // =========================
            // User & Role
            // =========================
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithMany()
                .HasForeignKey(u => u.EmployeeId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<LoginHistory>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);

            // =========================
            // Support & IT
            // =========================
            modelBuilder.Entity<SupportTicket>()
                .HasOne(st => st.Employee)
                .WithMany()
                .HasForeignKey(st => st.EmployeeId);

            modelBuilder.Entity<ITAsset>()
                .HasOne(it => it.Employee)
                .WithMany(e => e.ITAssets)
                .HasForeignKey(it => it.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // Document
            // =========================
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Employee)
                .WithMany()
                .HasForeignKey(d => d.EmployeeId);

            // =========================
            // Company & Branch
            // =========================
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Company)
                .WithMany()
                .HasForeignKey(b => b.CompanyId);

            // =========================
            // Notification
            // =========================
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Employee)
                .WithMany(e => e.Notifications)
                .HasForeignKey(n => n.EmployeeId);

            // =========================
            // Canteen
            // =========================
            modelBuilder.Entity<CanteenOrder>()
                .HasOne(c => c.Employee)
                .WithMany(e => e.CanteenOrders)
                .HasForeignKey(c => c.EmployeeId);

            // =========================
            // Transport
            // =========================
            modelBuilder.Entity<TransportVehicle>()
                .HasOne(tv => tv.Route)
                .WithMany()
                .HasForeignKey(tv => tv.RouteId);

            // =========================
            // Employee Onboarding
            // =========================

            modelBuilder.Entity<EmployeeOnboarding>()
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // Employee Document
            // =========================

            modelBuilder.Entity<EmployeeDocument>()
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // shift and employee
            // =========================
            modelBuilder.Entity<Employee>()
               .HasOne(x => x.Shift)
               .WithMany(x => x.Employees)
               .HasForeignKey(x => x.ShiftId)
               .OnDelete(DeleteBehavior.Restrict);
        }

    }
}