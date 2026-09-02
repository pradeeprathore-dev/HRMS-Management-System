using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using HRMSWEB.Models;
using HRMSWEB.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDashboardService _dashboardService;
        private readonly IWebHostEnvironment _environment;

        public HomeController(
            IHttpClientFactory httpClientFactory,
            IDashboardService dashboardService,
            IWebHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _dashboardService = dashboardService;
            _environment = environment;
        }

        // =========================
        // EMPLOYEE LIST
        // =========================

        public async Task<IActionResult> Index(
    string searchText = "",
    int pageNumber = 1)
        {
            // GET TOKEN
            var token =
                HttpContext.Session.GetString("JWToken");

            var role =
    HttpContext.Session.GetString("Role");

            ViewBag.Role = role;

            var employeeId =
                HttpContext.Session.GetInt32("EmployeeId");

            ViewBag.EmployeeId = employeeId;

            // NOT LOGGED IN
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            // CREATE CLIENT
            var client =
                _httpClientFactory.CreateClient();

            // ADD JWT TOKEN
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            // API CALL WITH SEARCH + PAGINATION
            var response =
                await client.GetAsync(
                    $"https://localhost:7212/api/Employee?PageNumber={pageNumber}&PageSize=10&searchText={searchText}");

            // FAIL
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<EmployeeViewModel>());
            }

            // JSON DATA
            var jsonData =
                await response.Content.ReadAsStringAsync();

            // DESERIALIZE
            dynamic result =
                JsonConvert.DeserializeObject(jsonData);

            // GET DATA
            var employeeData =
                JsonConvert.DeserializeObject
                <List<EmployeeViewModel>>
                (result.data.ToString());

            // VIEWBAG
            ViewBag.SearchText = searchText;
            ViewBag.PageNumber = pageNumber;

            // RETURN VIEW
            return View(employeeData);
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
                return RedirectToAction("Login", "Auth");
            }

            var client =
                _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            // =========================
            // API CALLS
            // =========================

            // DEPARTMENTS API
            var deptResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Department");

            // DESIGNATIONS API
            var desigResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Designation");

            // SHIFTS API
            var shiftResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Shift/Dropdown");

            // =========================
            // CREATE MODEL
            // =========================

            var model =
                new CreateEmployeeViewModel();

            // =========================
            // LOAD DEPARTMENTS
            // =========================

            if (deptResponse.IsSuccessStatusCode)
            {
                var deptJson =
                    await deptResponse.Content
                        .ReadAsStringAsync();

                var departments =
                    JsonConvert.DeserializeObject<List<DepartmentViewModel>>
                    (deptJson) ?? new List<DepartmentViewModel>();

                model.Departments =
                    departments.Select(x =>
                        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name
                        })
                    .ToList();
            }

            // =========================
            // LOAD DESIGNATIONS
            // =========================

            if (desigResponse.IsSuccessStatusCode)
            {
                var desigJson =
                    await desigResponse.Content
                        .ReadAsStringAsync();

                var designations =
                    JsonConvert.DeserializeObject<List<DesignationViewModel>>
                    (desigJson) ?? new List<DesignationViewModel>();

                model.Designations =
                    designations.Select(x =>
                        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Title
                        })
                    .ToList();
            }

            // =========================
            // LOAD SHIFTS
            // =========================

            if (shiftResponse.IsSuccessStatusCode)
            {
                var shiftJson =
                    await shiftResponse.Content
                        .ReadAsStringAsync();

                dynamic shiftResult =
                    JsonConvert.DeserializeObject(shiftJson);

                string shiftDataJson =
                    shiftResult.data.ToString();

                var shifts =
                    JsonConvert.DeserializeObject<List<ShiftDropdownViewModel>>
                    (shiftDataJson)
                    ?? new List<ShiftDropdownViewModel>();

                model.Shifts =
                    shifts.Select(x =>
                        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name
                        })
                    .ToList();
            }

            return View(model);
        }

        // =========================
        // SAVE EMPLOYEE
        // =========================

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateEmployeeViewModel model)
        {
            // TOKEN
            var token =
                HttpContext.Session.GetString("JWToken");

            // NOT LOGGED IN
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Token Missing";

                return View(model);
            }

            // CLIENT
            var client =
                _httpClientFactory.CreateClient();

            // ADD TOKEN
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
            string fileName = null;

            if (model.ImageFile != null)
            {
                string uploadFolder =
                    Path.Combine(
                        _environment.WebRootPath,
                        "uploads");

                fileName =
                    Guid.NewGuid().ToString()
                    + "_"
                    + model.ImageFile.FileName;

                string filePath =
                    Path.Combine(
                        uploadFolder,
                        fileName);

                using (var fileStream =
                    new FileStream(
                        filePath,
                        FileMode.Create))
                {
                    await model.ImageFile
                        .CopyToAsync(fileStream);
                }

                model.ProfileImage =
                    "/uploads/" + fileName;
            }

            // SERIALIZE MODEL
            var json =
                JsonConvert.SerializeObject(model);

            // JSON CONTENT
            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            // API CALL
            var response =
                await client.PostAsync(
                    "https://localhost:7212/api/Employee",
                    content);

            // SUCCESS
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // ERROR
            var result =
                await response.Content.ReadAsStringAsync();

            ViewBag.Error = result;

            return View(model);
        }

        // =========================
        // EDIT PAGE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // TOKEN
            var token =
                HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // CLIENT
            var client =
                _httpClientFactory.CreateClient();

            // ADD TOKEN
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            // =========================
            // GET EMPLOYEE
            // =========================

            var response =
                await client.GetAsync(
                    $"https://localhost:7212/api/Employee/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(jsonData);

            var employee =
                JsonConvert.DeserializeObject<CreateEmployeeViewModel>(
                    result.data.ToString());

            // =========================
            // GET DEPARTMENTS
            // =========================

            var deptResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Department");

            if (deptResponse.IsSuccessStatusCode)
            {
                var deptJson =
                    await deptResponse.Content.ReadAsStringAsync();

                var departments =
                    JsonConvert.DeserializeObject<List<DepartmentViewModel>>
                    (deptJson)
                    ?? new List<DepartmentViewModel>();

                employee.Departments =
     departments.Select(x =>
         new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
         {
             Value = x.Id.ToString(),
             Text = x.Name
         })
     .ToList();
            }

            // =========================
            // GET DESIGNATIONS
            // =========================

            var desigResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Designation");

            if (desigResponse.IsSuccessStatusCode)
            {
                var desigJson =
                    await desigResponse.Content.ReadAsStringAsync();

                var designations =
                    JsonConvert.DeserializeObject<List<DesignationViewModel>>
                    (desigJson)
                    ?? new List<DesignationViewModel>();

                employee.Designations =
    designations.Select(x =>
        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Title
        })
    .ToList();
            }

            // =========================
            // GET SHIFTS
            // =========================

            var shiftResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Shift/Dropdown");

            if (shiftResponse.IsSuccessStatusCode)
            {
                var shiftJson =
                    await shiftResponse.Content.ReadAsStringAsync();

                dynamic shiftResult =
                    JsonConvert.DeserializeObject(shiftJson);

                string shiftDataJson =
                    shiftResult.data.ToString();

                var shifts =
                    JsonConvert.DeserializeObject<List<ShiftDropdownViewModel>>(
                        shiftDataJson)
                    ?? new List<ShiftDropdownViewModel>();

                employee.Shifts =
                    shifts.Select(x =>
                        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name,
                            Selected = employee.ShiftId == x.Id
                        })
                    .ToList();
            }

            return View(employee);
        }

        // =========================
        // UPDATE EMPLOYEE
        // =========================

        [HttpPost]
        public async Task<IActionResult> Edit(
            CreateEmployeeViewModel model)
        {
            // TOKEN
            var token =
                HttpContext.Session.GetString("JWToken");

            // CLIENT
            var client =
                _httpClientFactory.CreateClient();

            // ADD TOKEN
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            // SERIALIZE
            var json =
                JsonConvert.SerializeObject(model);

            // CONTENT
            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            // PUT API
            var response =
                await client.PutAsync(
                    $"https://localhost:7212/api/Employee/{model.Id}",
                    content);

            // RESPONSE
            var result =
                await response.Content.ReadAsStringAsync();

            // DEBUG
            Console.WriteLine(result);

            // SUCCESS
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // ERROR
            ViewBag.Error = result;

            return View(model);
        }

        // =========================
        // DELETE EMPLOYEE
        // =========================

        public async Task<IActionResult> Delete(int id)
        {
            // TOKEN
            var token =
                HttpContext.Session.GetString("JWToken");

            // CLIENT
            var client =
                _httpClientFactory.CreateClient();

            // ADD TOKEN
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            // DELETE API
            await client.DeleteAsync(
                $"https://localhost:7212/api/Employee/{id}");

            // REDIRECT
            return RedirectToAction("Index");
        }
        // =========================
        // DASHBOARD
        // =========================

        public async Task<IActionResult> Dashboard()
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
                    "https://localhost:7212/api/Dashboard");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new DashboardViewModel());
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            var data =
                JsonConvert.DeserializeObject
                <DashboardViewModel>(jsonData);

            var pageModel =
    new DashboardPageViewModel();

            pageModel.Summary = data;
            var chartResponse =
    await client.GetAsync(
        "https://localhost:7212/api/Dashboard/LeaveChart");

            if (chartResponse.IsSuccessStatusCode)
            {
                var chartJson =
                    await chartResponse.Content.ReadAsStringAsync();

                pageModel.LeaveChart =
                    JsonConvert.DeserializeObject
                    <LeaveChartViewModel>(chartJson);
            }

            // =========================
            // ATTENDANCE CHART
            // =========================

            var attendanceResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/AttendanceChart");

            if (attendanceResponse.IsSuccessStatusCode)
            {
                var attendanceJson =
                    await attendanceResponse.Content
                    .ReadAsStringAsync();

                pageModel.AttendanceChart =
                    JsonConvert.DeserializeObject
                    <AttendanceChartViewModel>(
                        attendanceJson);
            }
            var recentLeaveResponse =
    await client.GetAsync(
        "https://localhost:7212/api/Dashboard/RecentLeaves");

            if (recentLeaveResponse.IsSuccessStatusCode)
            {
                var recentLeaveJson =
                    await recentLeaveResponse.Content
                    .ReadAsStringAsync();

                pageModel.RecentLeaves =
                    JsonConvert.DeserializeObject
                    <List<RecentLeaveViewModel>>
                    (recentLeaveJson);
            }
            // =========================
            // RECENT EMPLOYEES
            // =========================

            var recentEmployeeResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/RecentEmployees");

            if (recentEmployeeResponse.IsSuccessStatusCode)
            {
                var recentEmployeeJson =
                    await recentEmployeeResponse.Content
                    .ReadAsStringAsync();

                pageModel.RecentEmployees =
                    JsonConvert.DeserializeObject
                    <List<RecentEmployeeViewModel>>
                    (recentEmployeeJson);
            }
            // =========================
            // UPCOMING HOLIDAYS
            // =========================

            var holidayResponse =
                await client.GetAsync(
                    "https://localhost:7212/api/Dashboard/UpcomingHolidays");

            if (holidayResponse.IsSuccessStatusCode)
            {
                var holidayJson =
                    await holidayResponse.Content
                    .ReadAsStringAsync();

                pageModel.UpcomingHolidays =
                    JsonConvert.DeserializeObject
                    <List<UpcomingHolidayViewModel>>
                    (holidayJson);
            }

            var role = HttpContext.Session.GetString("Role");

            if (role == "2")
            {
                var employeeId =
                    HttpContext.Session.GetInt32("EmployeeId");

                return RedirectToAction(
                    "Profile",
                    new { id = employeeId });
            }

            return View(pageModel);
        }


        // =========================
        // DOWNLOAD PDF
        // =========================

        public async Task<IActionResult> DownloadPdf()
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
                    "https://localhost:7212/api/Employee");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index");
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(jsonData);

            var employees =
                JsonConvert.DeserializeObject
                <List<EmployeeViewModel>>
                (result.data.ToString());

            using (MemoryStream stream =
                new MemoryStream())
            {
                Document pdfDoc =
                    new Document(PageSize.A4);

                PdfWriter.GetInstance(
                    pdfDoc,
                    stream).CloseStream = false;

                pdfDoc.Open();

                Paragraph title =
                    new Paragraph(
                        "HRMS Employee Report");

                title.SpacingAfter = 20f;

                pdfDoc.Add(title);

                PdfPTable table =
                    new PdfPTable(4);

                table.WidthPercentage = 100;

                table.AddCell("Id");
                table.AddCell("Name");
                table.AddCell("Department");
                table.AddCell("Salary");

                foreach (var emp in employees)
                {
                    table.AddCell(emp.Id.ToString());

                    table.AddCell(
                        emp.FirstName + " " +
                        emp.LastName);

                    table.AddCell(
                        emp.Department);

                    table.AddCell(
                        emp.Salary.ToString());
                }

                pdfDoc.Add(table);

                pdfDoc.Close();

                stream.Position = 0;

                return File(
                    stream.ToArray(),
                    "application/pdf",
                    "EmployeeReport.pdf");
            }
        }
        // =========================
        // DOWNLOAD EXCEL
        // =========================

        public async Task<IActionResult> DownloadExcel()
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
                    "https://localhost:7212/api/Employee");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index");
            }

            var jsonData =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(jsonData);

            var employees =
                JsonConvert.DeserializeObject
                <List<EmployeeViewModel>>
                (result.data.ToString());

            using (var workbook =
                new XLWorkbook())
            {
                var worksheet =
                    workbook.Worksheets.Add(
                        "Employees");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "First Name";
                worksheet.Cell(1, 3).Value = "Last Name";
                worksheet.Cell(1, 4).Value = "Department";
                worksheet.Cell(1, 5).Value = "Salary";

                int row = 2;

                foreach (var emp in employees)
                {
                    worksheet.Cell(row, 1).Value = emp.Id;
                    worksheet.Cell(row, 2).Value = emp.FirstName;
                    worksheet.Cell(row, 3).Value = emp.LastName;
                    worksheet.Cell(row, 4).Value = emp.Department;
                    worksheet.Cell(row, 5).Value = emp.Salary;

                    row++;
                }

                using (var stream =
                    new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    var content =
                        stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "EmployeeReport.xlsx");
                }
            }
        }

        // =========================
        // PRIVACY
        // =========================

        public IActionResult Privacy()
        {
            return View();
        }

        // =========================
        // ERROR
        // =========================

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId =
                    Activity.Current?.Id ??
                    HttpContext.TraceIdentifier
            });

        }
        // =========================
        // EMPLOYEE PROFILE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Profile(
            int id)
        {
            var token =
                HttpContext.Session.GetString(
                    "JWToken");

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
                    $"https://localhost:7212/api/Employee/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index");
            }

            var jsonData =
                await response.Content
                .ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(
                    jsonData);

            var employee =
                JsonConvert.DeserializeObject
                <EmployeeViewModel>
                (result.data.ToString());

            return View(employee);
        }
    }
}