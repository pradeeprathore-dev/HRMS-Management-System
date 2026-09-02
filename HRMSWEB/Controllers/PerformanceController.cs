using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PerformanceController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // =========================
        // ALL PERFORMANCE REVIEWS
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
                    "https://localhost:7212/api/PerformanceReview");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<PerformanceReviewViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var data =
                JsonConvert.DeserializeObject
                <List<PerformanceReviewViewModel>>
                (result.data.ToString());

            return View(data);
        }
        // =========================
        // CREATE PAGE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Create()
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
                    "https://localhost:7212/api/Employee?PageNumber=1&PageSize=1000");

            var model =
                new CreatePerformanceReviewViewModel();

            if (response.IsSuccessStatusCode)
            {
                var json =
                    await response.Content.ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(json);

                List<EmployeeViewModel> employees =
     JsonConvert.DeserializeObject<List<EmployeeViewModel>>
     (result.data.ToString());

                model.Employees =
                    employees.Select(x =>
                        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = $"{x.Id} - {x.FirstName} {x.LastName}"
                        }).ToList();
            }

            return View(model);
        }
        // =========================
        // CREATE PERFORMANCE REVIEW
        // =========================

        [HttpPost]
        public async Task<IActionResult> Create(CreatePerformanceReviewViewModel model)
        {
            var token =
                HttpContext.Session.GetString("JWToken");
            Console.WriteLine("PERFORMANCE TOKEN = " + token);

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

            var json =
                JsonConvert.SerializeObject(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PostAsync(
                    "https://localhost:7212/api/PerformanceReview",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error =
                "Performance Review creation failed.";

            return View(model);
        }
        // =========================
        // EDIT PAGE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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
                    $"https://localhost:7212/api/PerformanceReview/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var model =
                JsonConvert.DeserializeObject<UpdatePerformanceReviewViewModel>(
                    result.data.ToString());

            return View(model);
        }
        // =========================
        // UPDATE PERFORMANCE REVIEW
        // =========================

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            UpdatePerformanceReviewViewModel model)
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

            var json =
                JsonConvert.SerializeObject(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PutAsync(
                    $"https://localhost:7212/api/PerformanceReview/{id}",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error =
                "Update failed.";

            return View(model);
        }
        // =========================
        // DELETE PERFORMANCE REVIEW
        // =========================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
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
                await client.DeleteAsync(
                    $"https://localhost:7212/api/PerformanceReview/{id}");

            return RedirectToAction("Index");
        }
    }
}