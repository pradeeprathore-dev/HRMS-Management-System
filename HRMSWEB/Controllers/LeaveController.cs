using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace HRMSWEB.Controllers
{
    public class LeaveController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeaveController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // APPLY LEAVE PAGE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Apply()
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            var employeeId =
                HttpContext.Session.GetInt32("EmployeeId");

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.GetAsync(
                    $"https://localhost:7212/api/LeaveType/Balance/{employeeId}");

            var model =
                new LeaveRequestViewModel();

            if (response.IsSuccessStatusCode)
            {
                var json =
                    await response.Content.ReadAsStringAsync();

                List<LeaveTypeViewModel> leaveTypes =
                    JsonConvert.DeserializeObject
                    <List<LeaveTypeViewModel>>(json);

                model.LeaveTypes =
                    leaveTypes.Select(x =>
                        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = $"{x.Name} ({x.RemainingDays} Days Left)"
                        }).ToList();
            }

            return View(model);
        }

        // =========================
        // APPLY LEAVE POST
        // =========================

        [HttpPost]
        public async Task<IActionResult> Apply(
            LeaveRequestViewModel model)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

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

            var jsonData =
                JsonConvert.SerializeObject(model);

            var content =
                new StringContent(
                    jsonData,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PostAsync(
                    "https://localhost:7212/api/Leave/Apply",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "MyLeaves",
                    new { employeeId = model.EmployeeId });
            }

            var errorMessage =
    await response.Content.ReadAsStringAsync();

            ViewBag.Error =
                errorMessage;

            return View(model);
        }

        // =========================
        // MY LEAVES
        // =========================

        [HttpGet]
        public async Task<IActionResult> MyLeaves(
            int employeeId)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

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

            var response =
                await client.GetAsync(
                    $"https://localhost:7212/api/Leave/MyLeaves/{employeeId}");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<LeaveResponseViewModel>());
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(jsonData);

            var leaves =
                JsonConvert.DeserializeObject
                <List<LeaveResponseViewModel>>
                (result.data.ToString());

            return View(leaves);
        }

        // =========================
        // ALL LEAVE REQUESTS
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token =
                HttpContext.Session.GetString("JWToken");

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

            var response =
                await client.GetAsync(
                    "https://localhost:7212/api/Leave");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<LeaveResponseViewModel>());
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(jsonData);

            var leaves =
                JsonConvert.DeserializeObject
                <List<LeaveResponseViewModel>>
                (result.data.ToString());

            return View(leaves);
        }

        // =========================
        // APPROVE LEAVE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Approve(
            int id)
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
                $"https://localhost:7212/api/Leave/Approve/{id}",
                null);

            return RedirectToAction("Index");
        }

        // =========================
        // REJECT LEAVE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Reject(
            int id)
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
                $"https://localhost:7212/api/Leave/Reject/{id}",
                null);

            return RedirectToAction("Index");
        }
    }
}