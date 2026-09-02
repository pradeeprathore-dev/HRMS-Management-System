using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AttendanceController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // MY ATTENDANCE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index(
    int employeeId)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            var role =
                HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            HttpResponseMessage response;

            if (role == "1")
            {
                response =
                    await client.GetAsync(
                        "https://localhost:7212/api/Attendance");
            }
            else
            {
                response =
                    await client.GetAsync(
                        $"https://localhost:7212/api/Attendance/MyAttendance/{employeeId}");
            }

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<AttendanceViewModel>());
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(jsonData);

            var data =
                JsonConvert.DeserializeObject
                <List<AttendanceViewModel>>
                (result.data.ToString());

            return View(data);
        }

        // =========================
        // PUNCH IN
        // =========================

        [HttpGet]
        public async Task<IActionResult> PunchIn(
            int employeeId)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var body =
                new
                {
                    EmployeeId = employeeId
                };

            var jsonData =
                JsonConvert.SerializeObject(body);

            var content =
                new StringContent(
                    jsonData,
                    Encoding.UTF8,
                    "application/json");

            await client.PostAsync(
                "https://localhost:7212/api/Attendance/PunchIn",
                content);
            TempData["Success"] =
    "Punch In Successful";

            return RedirectToAction(
                "Index",
                new { employeeId = employeeId });
        }

        // =========================
        // PUNCH OUT
        // =========================

        [HttpGet]
        public async Task<IActionResult> PunchOut(
            int employeeId)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            await client.PutAsync(
                $"https://localhost:7212/api/Attendance/PunchOut/{employeeId}",
                null);
            TempData["Success"] =
    "Punch Out Successful";

            return RedirectToAction(
                "Index",
                new { employeeId = employeeId });
        }
    }
}