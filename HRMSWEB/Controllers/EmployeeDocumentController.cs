using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HRMSWEB.Controllers
{
    public class EmployeeDocumentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeDocumentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            // JWT Token
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Http Client
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // API Call
            var response = await client.GetAsync(
                "https://localhost:7212/api/EmployeeDocument");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<EmployeeDocumentViewModel>());
            }

            // Read Json
            var jsonData = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(jsonData);

            var documents =
                JsonConvert.DeserializeObject<List<EmployeeDocumentViewModel>>
                (result.data.ToString());

            return View(documents);
        }
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

            var response =
    await client.GetAsync("https://localhost:7212/api/Employee/Dropdown");

            var model = new CreateEmployeeDocumentViewModel();

            if (response.IsSuccessStatusCode)
            {
                var jsonData =
                    await response.Content.ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(jsonData);

                List<EmployeeDropdownViewModel> employees =
     JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
     (result.data.ToString())!;

                model.Employees =
     employees.Select(x => new SelectListItem
     {
         Value = x.Id.ToString(),
         Text = x.Name
     }).ToList();
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var formData = new MultipartFormDataContent();

            formData.Add(
                new StringContent(model.EmployeeId.ToString()),
                "EmployeeId");

            formData.Add(
                new StringContent(model.DocumentType ?? ""),
                "DocumentType");

            formData.Add(
                new StringContent(model.Remarks ?? ""),
                "Remarks");

            if (model.File != null && model.File.Length > 0)
            {
                var streamContent =
                    new StreamContent(model.File.OpenReadStream());

                streamContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(model.File.ContentType);

                formData.Add(
                    streamContent,
                    "File",
                    model.File.FileName);
            }

            var response = await client.PostAsync(
                "https://localhost:7212/api/EmployeeDocument",
                formData);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Document Uploaded Successfully.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Document Upload Failed.";

            return View(model);
        }
        public async Task<IActionResult> Preview(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(
                $"https://localhost:7212/api/EmployeeDocument/Preview/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Document not found.";

                return RedirectToAction(nameof(Index));
            }

            var bytes = await response.Content.ReadAsByteArrayAsync();

            var contentType =
                response.Content.Headers.ContentType?.MediaType
                ?? "application/octet-stream";

            return File(bytes, contentType);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // ============================
            // Get Document Details
            // ============================
            var documentResponse =
                await client.GetAsync($"https://localhost:7212/api/EmployeeDocument/{id}");

            if (!documentResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Document not found.";
                return RedirectToAction(nameof(Index));
            }

            var documentJson =
                await documentResponse.Content.ReadAsStringAsync();

            dynamic documentResult =
                JsonConvert.DeserializeObject(documentJson);

            var document =
                JsonConvert.DeserializeObject<EmployeeDocumentViewModel>
                (documentResult.data.ToString());

            // ============================
            // Get Employee Dropdown
            // ============================
            var employeeResponse =
                await client.GetAsync("https://localhost:7212/api/Employee/Dropdown");

            List<SelectListItem> employees = new();

            if (employeeResponse.IsSuccessStatusCode)
            {
                var employeeJson =
                    await employeeResponse.Content.ReadAsStringAsync();

                dynamic employeeResult =
                    JsonConvert.DeserializeObject(employeeJson);

                List<EmployeeDropdownViewModel> employeeList =
                    JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
                    (employeeResult.data.ToString());

                employees = employeeList.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();
            }

            var model = new UpdateEmployeeDocumentViewModel
            {
                Id = document.Id,
                EmployeeId = document.EmployeeId,
                DocumentType = document.DocumentType,
                Remarks = document.Remarks,
                IsVerified = document.IsVerified,
                Employees = employees
            };

            ViewBag.CurrentFile = document.FileName;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateEmployeeDocumentViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var formData = new MultipartFormDataContent();

            formData.Add(
                new StringContent(model.Id.ToString()),
                "Id");

            formData.Add(
                new StringContent(model.EmployeeId.ToString()),
                "EmployeeId");

            formData.Add(
                new StringContent(model.DocumentType ?? ""),
                "DocumentType");

            formData.Add(
                new StringContent(model.IsVerified.ToString()),
                "IsVerified");

            formData.Add(
                new StringContent(model.Remarks ?? ""),
                "Remarks");

            if (model.File != null && model.File.Length > 0)
            {
                var streamContent = new StreamContent(model.File.OpenReadStream());

                streamContent.Headers.ContentType =
                    new MediaTypeHeaderValue(model.File.ContentType);

                formData.Add(
                    streamContent,
                    "File",
                    model.File.FileName);
            }

            var response = await client.PutAsync(
                $"https://localhost:7212/api/EmployeeDocument/{model.Id}",
                formData);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Document Updated Successfully.";

                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Document Update Failed.";

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(
                $"https://localhost:7212/api/EmployeeDocument/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Document Deleted Successfully.";
            }
            else
            {
                TempData["Error"] = "Unable to delete document.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}