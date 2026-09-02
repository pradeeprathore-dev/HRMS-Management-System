using HRMSWEB.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRMSWEB.Controllers
{
    public class ITAssetController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ITAssetController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // LIST
        // =========================

        public async Task<IActionResult> Index(string searchText = "", int pageNumber = 1)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var role = HttpContext.Session.GetString("Role");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            string url;

            if (role == "1")
            {
                url = $"https://localhost:7212/api/ITAsset?PageNumber={pageNumber}&PageSize=10&SearchText={searchText}";
            }
            else
            {
                url = $"https://localhost:7212/api/ITAsset/MyAssets?PageNumber={pageNumber}&PageSize=10&SearchText={searchText}";
            }

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return View(new List<ITAssetViewModel>());

            var json = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json);

            var assets = JsonConvert.DeserializeObject<List<ITAssetViewModel>>
                (result.data.ToString());

            ViewBag.SearchText = searchText;

            return View(assets);
        }

        // =========================
        // CREATE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var employeeResponse =
                await client.GetAsync("https://localhost:7212/api/Employee/Dropdown");

            var model = new CreateITAssetViewModel();

            if (employeeResponse.IsSuccessStatusCode)
            {
                var json = await employeeResponse.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(json);

                var employees = JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
                    (result.data.ToString());

                List<EmployeeDropdownViewModel> employeeList =
     JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
     (result.data.ToString()) ?? new List<EmployeeDropdownViewModel>();

                model.Employees = employeeList
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToList();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateITAssetViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            // Request.Form se values lo
            model.AssetName = Request.Form["AssetName"];
            model.AssetType = Request.Form["AssetType"];
            model.AssetCode = Request.Form["AssetCode"];
            model.Brand = Request.Form["Brand"];
            model.Model = Request.Form["Model"];
            model.SerialNumber = Request.Form["SerialNumber"];
            model.Status = Request.Form["Status"];
            model.Remarks = Request.Form["Remarks"];

            int.TryParse(Request.Form["EmployeeId"], out int parsedEmployeeId);
            model.EmployeeId = parsedEmployeeId;

            decimal.TryParse(Request.Form["PurchasePrice"], out decimal parsedPrice);
            model.PurchasePrice = parsedPrice;

            DateTime.TryParse(Request.Form["PurchaseDate"], out DateTime parsedDate);
            model.PurchaseDate = parsedDate;

            if (model.EmployeeId <= 0)
            {
                ModelState.AddModelError(nameof(model.EmployeeId), "Please select an employee.");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;
            string sentJson = "";
            string responseBody = "";

            //if (ModelState.IsValid)
            //{
            //    var payload = new
            //    {
            //        model.AssetName,
            //        model.AssetType,
            //        model.AssetCode,
            //        model.Brand,
            //        model.Model,
            //        model.SerialNumber,
            //        model.PurchaseDate,
            //        model.PurchasePrice,
            //        model.Status,
            //        model.Remarks,
            //        model.EmployeeId
            //    };

            //    sentJson = JsonConvert.SerializeObject(payload);

            //    var content = new StringContent(
            //        sentJson,
            //        Encoding.UTF8,
            //        "application/json");

            //    Console.WriteLine(sentJson);
            //    Console.WriteLine("========== JSON ==========");
            //    Console.WriteLine(sentJson);
            //    Console.WriteLine("========== TOKEN ==========");
            //    Console.WriteLine(token);
            //    Console.WriteLine("===========================");
            //    System.Diagnostics.Debug.WriteLine("====== JSON ======");
            //    System.Diagnostics.Debug.WriteLine(sentJson);
            //    throw new Exception("POST CALL TAK AA GAYA");

            //    response = await client.PostAsync(
            //        "https://localhost:7212/api/ITAsset",
            //        content);

            //    responseBody = await response.Content.ReadAsStringAsync();

            //    if (response.IsSuccessStatusCode)
            //    {
            //        TempData["Success"] = "IT Asset Created Successfully";
            //        return RedirectToAction(nameof(Index));
            //    }
            //}
            //else
            //{
            //    response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            //}
          
                var payload = new
                {
                    model.AssetName,
                    model.AssetType,
                    model.AssetCode,
                    model.Brand,
                    model.Model,
                    model.SerialNumber,
                    model.PurchaseDate,
                    model.PurchasePrice,
                    model.Status,
                    model.Remarks,
                    model.EmployeeId
                };

                sentJson = JsonConvert.SerializeObject(payload);

                var content = new StringContent(
                    sentJson,
                    Encoding.UTF8,
                    "application/json");

                //Console.WriteLine(sentJson);
                //Console.WriteLine("========== JSON ==========");
                //Console.WriteLine(sentJson);
                //Console.WriteLine("========== TOKEN ==========");
                //Console.WriteLine(token);
                //Console.WriteLine("===========================");
                //System.Diagnostics.Debug.WriteLine("====== JSON ======");
                //System.Diagnostics.Debug.WriteLine(sentJson);
                //throw new Exception("POST CALL TAK AA GAYA");

                response = await client.PostAsync(
                    "https://localhost:7212/api/ITAsset",
                    content);

                responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "IT Asset Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
           





            //commented end 
            var error = responseBody;

            string errorMessage = "Kuch galat ho gaya, asset save nahi ho paaya.";

            try
            {
                dynamic errorResult = JsonConvert.DeserializeObject(error);

                if (errorResult?.Details != null)
                    errorMessage = errorResult.Details.ToString();
                else if (errorResult?.message != null)
                    errorMessage = errorResult.message.ToString();
                else if (errorResult?.title != null)
                    errorMessage = errorResult.title.ToString();
                else if (errorResult?.Message != null)
                    errorMessage = errorResult.Message.ToString();
            }
            catch
            {
            }

            errorMessage =
                $"[DEBUG]\n\n" +
                $"Sent JSON:\n{sentJson}\n\n" +
                $"Status: {(int)response.StatusCode} {response.StatusCode}\n\n" +
                $"Response:\n{(string.IsNullOrWhiteSpace(error) ? "(empty body)" : error)}";

            ModelState.AddModelError(string.Empty, errorMessage);

            // Reload Employee Dropdown
            var employeeResponse =
                await client.GetAsync("https://localhost:7212/api/Employee/Dropdown");

            if (employeeResponse.IsSuccessStatusCode)
            {
                var empJson = await employeeResponse.Content.ReadAsStringAsync();

                dynamic empResult = JsonConvert.DeserializeObject(empJson);

                List<EmployeeDropdownViewModel> employeeList =
                    JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
                    (empResult.data.ToString()) ?? new List<EmployeeDropdownViewModel>();

                model.Employees = employeeList
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToList();
            }

            return View(model);
        }

        // =========================
        // EDIT
        // =========================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response =
                await client.GetAsync($"https://localhost:7212/api/ITAsset/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var json = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json);

            var model =
                JsonConvert.DeserializeObject<UpdateITAssetViewModel>
                (result.data.ToString());

            var employeeResponse =
                await client.GetAsync("https://localhost:7212/api/Employee/Dropdown");

            if (employeeResponse.IsSuccessStatusCode)
            {
                var empJson = await employeeResponse.Content.ReadAsStringAsync();

                dynamic empResult = JsonConvert.DeserializeObject(empJson);

                //var employees = JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
                //    (empResult.data.ToString());

                List<EmployeeDropdownViewModel> employeeList =
     JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
     (empResult.data.ToString()) ?? new List<EmployeeDropdownViewModel>();

                model.Employees = employeeList
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToList();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateITAssetViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);

            var content =
                new StringContent(json, Encoding.UTF8, "application/json");

            var response =
                await client.PutAsync(
                    $"https://localhost:7212/api/ITAsset/{model.Id}",
                    content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            // Reload Employee Dropdown
            var employeeResponse =
                await client.GetAsync("https://localhost:7212/api/Employee/Dropdown");

            if (employeeResponse.IsSuccessStatusCode)
            {
                var empJson = await employeeResponse.Content.ReadAsStringAsync();

                dynamic empResult = JsonConvert.DeserializeObject(empJson);

                var employees = JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
                    (empResult.data.ToString());

                List<EmployeeDropdownViewModel> employeeList =
     JsonConvert.DeserializeObject<List<EmployeeDropdownViewModel>>
     (empResult.data.ToString()) ?? new List<EmployeeDropdownViewModel>();

                model.Employees = employeeList
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToList();
            }

            return View(model);
        }

        // =========================
        // DELETE
        // =========================

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            await client.DeleteAsync(
                $"https://localhost:7212/api/ITAsset/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}