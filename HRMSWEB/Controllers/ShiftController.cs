using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class ShiftController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ShiftController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index(
    string searchText = "",
    int pageNumber = 1)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(
                $"https://localhost:7212/api/Shift?searchText={searchText}&pageNumber={pageNumber}");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ShiftViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var data =
                JsonConvert.DeserializeObject<List<ShiftViewModel>>
                (result.data.ToString());

            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateShiftViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                model.ShiftName,
                model.StartTime,
                model.EndTime,
                model.GraceMinutes,
                model.HalfDayTime,
                model.IsActive
            };

            var json = JsonConvert.SerializeObject(payload);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "https://localhost:7212/api/Shift",
                content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Shift Created Successfully";

                return RedirectToAction(nameof(Index));
            }

            var error =
                await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", error);

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(
                $"https://localhost:7212/api/Shift/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            var model =
                JsonConvert.DeserializeObject<UpdateShiftViewModel>(
                    result.data.ToString());

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateShiftViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                model.ShiftName,
                model.StartTime,
                model.EndTime,
                model.GraceMinutes,
                model.HalfDayTime,
                model.IsActive
            };

            var json = JsonConvert.SerializeObject(payload);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync(
                $"https://localhost:7212/api/Shift/{model.Id}",
                content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Shift Updated Successfully";

                return RedirectToAction(nameof(Index));
            }

            var error = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError("", error);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(
                $"https://localhost:7212/api/Shift/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Shift Deleted Successfully";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                TempData["Error"] = string.IsNullOrWhiteSpace(error)
                    ? "Unable to delete Shift."
                    : error;
            }

            return RedirectToAction(nameof(Index));
        }
    }

}