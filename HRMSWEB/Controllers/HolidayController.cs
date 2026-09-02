using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class HolidayController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HolidayController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // HOLIDAY LIST
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
                    "https://localhost:7212/api/Holiday");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<HolidayViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var holidays =
                JsonConvert.DeserializeObject<List<HolidayViewModel>>(
                    result.data.ToString());

            return View(holidays);
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
            CreateHolidayViewModel model)
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
                await client.PostAsync(
                    "https://localhost:7212/api/Holiday",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error =
                "Holiday creation failed.";

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

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.GetAsync(
                    $"https://localhost:7212/api/Holiday/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            var holiday =
                Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateHolidayViewModel>(
                    result.data.ToString());

            return View(holiday);
        }
        // =========================
        // EDIT POST
        // =========================

        [HttpPost]
        public async Task<IActionResult> Edit(
            UpdateHolidayViewModel model)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var json =
                Newtonsoft.Json.JsonConvert.SerializeObject(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PutAsync(
                    $"https://localhost:7212/api/Holiday/{model.Id}",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Holiday update failed.";

            return View(model);
        }
        // =========================
        // DELETE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token =
                HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.DeleteAsync(
                    $"https://localhost:7212/api/Holiday/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "Holiday delete failed.";
            }

            return RedirectToAction("Index");
        }
    }
}