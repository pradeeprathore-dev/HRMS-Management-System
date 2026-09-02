using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HRMSWEB.ViewComponents
{
    public class NotificationBellViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationBellViewComponent(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new NotificationBellViewModel();

            var token =
                HttpContext.Session.GetString("JWToken");

            var employeeId =
                HttpContext.Session.GetInt32("EmployeeId");

            if (!string.IsNullOrEmpty(token) &&
                employeeId != null)
            {
                var client =
                    _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);

                var response =
                    await client.GetAsync(
                        $"https://localhost:7212/api/Notification/Bell/{employeeId}");

                if (response.IsSuccessStatusCode)
                {
                    var json =
                        await response.Content.ReadAsStringAsync();

                    dynamic result =
                        JsonConvert.DeserializeObject(json);

                    model =
                        JsonConvert.DeserializeObject<NotificationBellViewModel>(
                            result.data.ToString());
                }
            }

            return View(model);
        }
    }
}