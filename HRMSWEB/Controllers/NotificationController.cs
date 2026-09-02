using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HRMSWEB.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationController(
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
                    $"https://localhost:7212/api/Notification/Employee/{employeeId}");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<NotificationViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json);

            List<NotificationViewModel> notifications =
                JsonConvert.DeserializeObject<List<NotificationViewModel>>(
                    result.data.ToString());

            return View(notifications);
        }
        // =========================
        // MARK AS READ
        // =========================

        [HttpGet]
        public async Task<IActionResult> MarkAsRead(
            int id)
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

            await client.PutAsync(
                $"https://localhost:7212/api/Notification/MarkAsRead/{id}",
                null);

            return RedirectToAction("Index");
        }
        // =========================
        // DELETE NOTIFICATION
        // =========================

        [HttpGet]
        public async Task<IActionResult> Delete(
            int id)
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
                $"https://localhost:7212/api/Notification/{id}");

            return RedirectToAction("Index");
        }
    }

}