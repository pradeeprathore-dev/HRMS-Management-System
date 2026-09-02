using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DepartmentController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

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
                    "https://localhost:7212/api/Department");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<DepartmentViewModel>());
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            var departments =
                JsonConvert.DeserializeObject
                <List<DepartmentViewModel>>(jsonData);

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(
    DepartmentViewModel model)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

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
                    "https://localhost:7212/api/Department",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.GetAsync(
                    $"https://localhost:7212/api/Department/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var department =
                JsonConvert.DeserializeObject
                <DepartmentViewModel>(json);

            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(
    int id,
    DepartmentViewModel model)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

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

            await client.PutAsync(
                $"https://localhost:7212/api/Department/{id}",
                content);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.DeleteAsync(
                    $"https://localhost:7212/api/Department/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Department deleted successfully.";
            }
            else
            {
                var error =
                    await response.Content.ReadAsStringAsync();

                TempData["Error"] = error;
            }

            return RedirectToAction("Index");
        }
    }
}