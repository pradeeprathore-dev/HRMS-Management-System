using HRMSWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace HRMSWEB.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IHttpClientFactory
            _httpClientFactory;

        public PayrollController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory =
                httpClientFactory;
        }

        // =========================
        // GENERATE PAYROLL PAGE
        // =========================

        [HttpGet]
        public async Task<IActionResult> Create()
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
                    "https://localhost:7212/api/Employee?PageNumber=1&PageSize=1000");

            var model =
                new CreatePayrollViewModel();

            if (response.IsSuccessStatusCode)
            {
                var jsonData =
                    await response.Content
                    .ReadAsStringAsync();

                var result =
    JsonConvert.DeserializeObject<dynamic>(
        jsonData);

                string employeeJson =
                    Convert.ToString(result.data);

                var employees =
                    JsonConvert.DeserializeObject<
                        List<EmployeeViewModel>>
                        (employeeJson);

                model.Employees =
                    employees.Select(x =>
                        new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = $"{x.Id} - {x.FirstName} {x.LastName}"
                        }).ToList();
            }

            return View(model);
        }

        // =========================
        // GENERATE PAYROLL
        // =========================

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePayrollViewModel model)
        {
            var token =
                HttpContext.Session
                .GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var client =
                _httpClientFactory
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var jsonData =
                JsonConvert.SerializeObject(
                    model);

            var content =
                new StringContent(
                    jsonData,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PostAsync(
                    "https://localhost:7212/api/Payroll",
                    content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index");
            }

            //ViewBag.Error =
            //    "Payroll generation failed";
            var error =
    await response.Content.ReadAsStringAsync();

            ViewBag.Error = error;

            return View(model);
        }

        // =========================
        // ALL PAYROLLS
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token =
                HttpContext.Session
                .GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var client =
                _httpClientFactory
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.GetAsync(
                    "https://localhost:7212/api/Payroll");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<PayrollViewModel>());
            }

            var jsonData =
                await response.Content
                .ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(
                    jsonData);

            var payrolls =
                JsonConvert.DeserializeObject
                <List<PayrollViewModel>>
                (result.data.ToString());

            return View(payrolls);
        }

        // =========================
        // MY PAYROLLS
        // =========================

        [HttpGet]
        public async Task<IActionResult> MyPayrolls(
            int employeeId)
        {
            var token =
                HttpContext.Session
                .GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var client =
                _httpClientFactory
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.GetAsync(
                    $"https://localhost:7212/api/Payroll/MyPayrolls/{employeeId}");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<PayrollViewModel>());
            }

            var jsonData =
                await response.Content
                .ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(
                    jsonData);

            var payrolls =
                JsonConvert.DeserializeObject
                <List<PayrollViewModel>>
                (result.data.ToString());

            return View(payrolls);
        }

        // =========================
        // DOWNLOAD PAYSLIP PDF
        // =========================

        [HttpGet]
        public async Task<IActionResult> DownloadPayslip(
            int id)
        {
            var token =
                HttpContext.Session
                .GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var client =
                _httpClientFactory
                .CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.GetAsync(
                    "https://localhost:7212/api/Payroll");

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

            List<PayrollViewModel> payrolls =
                JsonConvert.DeserializeObject
                <List<PayrollViewModel>>
                (result.data.ToString());

            PayrollViewModel payroll =
                payrolls.FirstOrDefault(
                    x => x.Id == id);

            if (payroll == null)
            {
                return RedirectToAction(
                    "Index");
            }

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
                        "Employee Payslip");

                title.SpacingAfter = 20f;

                pdfDoc.Add(title);

                pdfDoc.Add(
                    new Paragraph(
                        $"Employee: {payroll.EmployeeName}"));

                pdfDoc.Add(
                    new Paragraph(
                        $"Month: {payroll.Month}/{payroll.Year}"));

                pdfDoc.Add(
                    new Paragraph(
                        $"Basic Salary: {payroll.BasicSalary}"));

                pdfDoc.Add(
                    new Paragraph(
                        $"HRA: {payroll.HRA}"));

                pdfDoc.Add(
                    new Paragraph(
                        $"Bonus: {payroll.Bonus}"));

                pdfDoc.Add(
                    new Paragraph(
                        $"Deduction: {payroll.Deduction}"));

                pdfDoc.Add(
                    new Paragraph(
                        $"Net Salary: {payroll.NetSalary}"));

                pdfDoc.Close();

                stream.Position = 0;

                return File(
                    stream.ToArray(),
                    "application/pdf",
                    "Payslip.pdf");
            }
        }
        // =========================
        // APPROVE PAYROLL
        // =========================

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response =
                await client.PutAsync(
                    $"https://localhost:7212/api/Payroll/Approve/{id}",
                    null);

            return RedirectToAction("Index");
        }

        // =========================
        // MARK PAYROLL AS PAID
        // =========================

        [HttpPost]
        public async Task<IActionResult> MarkPaid(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response =
                await client.PutAsync(
                    $"https://localhost:7212/api/Payroll/MarkPaid/{id}",
                    null);

            return RedirectToAction("Index");
        }
    }
}