using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class DesignationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DesignationController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // LIST
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index()
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
                    "https://localhost:7212/api/Designation");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<DesignationViewModel>());
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            var designations =
                JsonConvert.DeserializeObject
                <List<DesignationViewModel>>(jsonData);

            return View(designations);
        }

        // =========================
        // CREATE GET
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        public async Task<IActionResult> Create(
            DesignationViewModel model)
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
                    "https://localhost:7212/api/Designation",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // =========================
        // EDIT GET
        // =========================

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
                    $"https://localhost:7212/api/Designation/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var json =
                await response.Content.ReadAsStringAsync();

            var designation =
                JsonConvert.DeserializeObject
                <DesignationViewModel>(json);

            return View(designation);
        }

        // =========================
        // EDIT POST
        // =========================

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            DesignationViewModel model)
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
                $"https://localhost:7212/api/Designation/{id}",
                content);

            return RedirectToAction("Index");
        }

        // =========================
        // DELETE
        // =========================

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

            await client.DeleteAsync(
                $"https://localhost:7212/api/Designation/{id}");

            return RedirectToAction("Index");
        }
    }
}