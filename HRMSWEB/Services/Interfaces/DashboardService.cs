using HRMSWEB.Models;
using HRMSWEB.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HRMSWEB.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<DashboardPageViewModel> GetDashboardDataAsync(string token)
        {
            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var pageModel =
                new DashboardPageViewModel();

            // =========================
            // SUMMARY
            // =========================

            var response =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard");

            if (response.IsSuccessStatusCode)
            {
                var json =
                    await response.Content.ReadAsStringAsync();

                pageModel.Summary =
                    JsonConvert.DeserializeObject<DashboardViewModel>(json);
            }

            // =========================
            // LEAVE CHART
            // =========================

            var leaveResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/LeaveChart");

            if (leaveResponse.IsSuccessStatusCode)
            {
                var json =
                    await leaveResponse.Content.ReadAsStringAsync();

                pageModel.LeaveChart =
                    JsonConvert.DeserializeObject<LeaveChartViewModel>(json);
            }

            // =========================
            // ATTENDANCE CHART
            // =========================

            var attendanceResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/AttendanceChart");

            if (attendanceResponse.IsSuccessStatusCode)
            {
                var json =
                    await attendanceResponse.Content.ReadAsStringAsync();

                pageModel.AttendanceChart =
                    JsonConvert.DeserializeObject<AttendanceChartViewModel>(json);
            }

            // =========================
            // RECENT LEAVES
            // =========================

            var leaveListResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/RecentLeaves");

            if (leaveListResponse.IsSuccessStatusCode)
            {
                var json =
                    await leaveListResponse.Content.ReadAsStringAsync();

                pageModel.RecentLeaves =
                    JsonConvert.DeserializeObject<List<RecentLeaveViewModel>>(json);
            }

            // =========================
            // RECENT EMPLOYEES
            // =========================

            var employeeResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/RecentEmployees");

            if (employeeResponse.IsSuccessStatusCode)
            {
                var json =
                    await employeeResponse.Content.ReadAsStringAsync();

                pageModel.RecentEmployees =
                    JsonConvert.DeserializeObject<List<RecentEmployeeViewModel>>(json);
            }

            // =========================
            // UPCOMING HOLIDAYS
            // =========================

            var holidayResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/UpcomingHolidays");

            if (holidayResponse.IsSuccessStatusCode)
            {
                var json =
                    await holidayResponse.Content.ReadAsStringAsync();

                pageModel.UpcomingHolidays =
                    JsonConvert.DeserializeObject<List<UpcomingHolidayViewModel>>(json);
            }

            return pageModel;
        }
    }
}