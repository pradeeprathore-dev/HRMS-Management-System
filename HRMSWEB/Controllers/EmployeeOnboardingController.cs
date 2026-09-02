using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class EmployeeOnboardingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeOnboardingController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // INDEX
        // =========================

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
                    "https://localhost:7212/api/EmployeeOnboarding");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<EmployeeOnboardingViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var data =
                JsonConvert.DeserializeObject<List<EmployeeOnboardingViewModel>>
                (
                    result.data.ToString()
                );

            return View(data);
        }

        // =========================
        // CREATE PAGE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var model = new CreateEmployeeOnboardingViewModel();

            var employeeResponse =
                await client.GetAsync("https://localhost:7212/api/Employee");

            if (employeeResponse.IsSuccessStatusCode)
            {
                var json =
                    await employeeResponse.Content.ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(json);

                var employees =
                    JsonConvert.DeserializeObject<List<EmployeeViewModel>>
                    (
                        result.data.ToString()
                    );

                model.Employees = new List<SelectListItem>();

                if (employees != null)
                {
                    foreach (var emp in employees)
                    {
                        model.Employees.Add(new SelectListItem
                        {
                            Value = emp.Id.ToString(),
                            Text = emp.FirstName + " " + emp.LastName
                        });
                    }
                }
            }

            return View(model);
        }


        // =========================
        // SAVE
        // =========================

        [HttpPost]
            public async Task<IActionResult> Create(
                CreateEmployeeOnboardingViewModel model)
            {
                var token =
                    HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(token))
                {
                    return View(model);
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
                        "https://localhost:7212/api/EmployeeOnboarding",
                        content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(
                        "Index");
                }

                ViewBag.Error =
                    await response.Content.ReadAsStringAsync();

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
                    $"https://localhost:7212/api/EmployeeOnboarding/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var model =
                JsonConvert.DeserializeObject<CreateEmployeeOnboardingViewModel>
                (
                    result.data.ToString()
                );

            var employeeResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Employee");

            if (employeeResponse.IsSuccessStatusCode)
            {
                var employeeJson =
                    await employeeResponse.Content.ReadAsStringAsync();

                dynamic employeeResult =
                    JsonConvert.DeserializeObject(employeeJson);

                var employees =
                    JsonConvert.DeserializeObject<List<EmployeeViewModel>>
                    (
                        employeeResult.data.ToString()
                    );

                model.Employees = new List<SelectListItem>();

                foreach (var emp in employees)
                {
                    model.Employees.Add(new SelectListItem
                    {
                        Value = emp.Id.ToString(),
                        Text = emp.FirstName + " " + emp.LastName
                    });
                }
            }

            return View(model);
        }

        // =========================
        // UPDATE
        // =========================

        [HttpPost]
        public async Task<IActionResult> Edit(
            CreateEmployeeOnboardingViewModel model)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return View(model);
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
                    $"https://localhost:7212/api/EmployeeOnboarding/{model.Id}",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error =
                await response.Content.ReadAsStringAsync();

            return View(model);
        }

        // =========================
        // DETAILS
        // =========================

        public async Task<IActionResult> Details(int id)
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
                    $"https://localhost:7212/api/EmployeeOnboarding/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var model =
                JsonConvert.DeserializeObject<EmployeeOnboardingViewModel>
                (
                    result.data.ToString()
                );

            return View(model);
        }

        // =========================
        // DELETE
        // =========================

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

            await client.DeleteAsync(
                $"https://localhost:7212/api/EmployeeOnboarding/{id}");

            return RedirectToAction("Index");
        }
    }
}