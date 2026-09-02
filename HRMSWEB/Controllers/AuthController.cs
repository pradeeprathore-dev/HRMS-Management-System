using HRMSWEB.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace HRMSWEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =========================
        // LOGIN PAGE
        // =========================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // LOGIN POST
        // =========================

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var client =
                _httpClientFactory.CreateClient();

            var jsonData =
                JsonConvert.SerializeObject(dto);

            var content =
                new StringContent(
                    jsonData,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PostAsync(
                    "https://localhost:7212/api/Auth/login",
                    content);

            if (response.IsSuccessStatusCode)
            {
                var result =
                    await response.Content.ReadAsStringAsync();

                var loginResponse =
                    JsonConvert.DeserializeObject<LoginResponseDto>(result);

                HttpContext.Session.SetString(
                    "JWToken",
                    loginResponse.Token);

                var handler =
                    new JwtSecurityTokenHandler();

                var jwtToken =
                    handler.ReadJwtToken(loginResponse.Token);

                var role =
                    jwtToken.Claims
                    .FirstOrDefault(x =>
                        x.Type.Contains("role"))
                    ?.Value;

                //throw new Exception("ROLE = " + role);

                HttpContext.Session.SetString(
                    "Role",
                    role);

                // =========================
                // SAVE EMPLOYEE ID
                // =========================

                HttpContext.Session.SetInt32(
                    "EmployeeId",
                    loginResponse.EmployeeId);

                if (role == "1")
                {
                    return RedirectToAction(
                        "Dashboard",
                        "Home");
                }
                else
                {
                    return RedirectToAction(
                        "Profile",
                        "Home",
                        new
                        {
                            id = loginResponse.EmployeeId
                        });
                }
            }

            ViewBag.Error =
                "Invalid Username or Password";

            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var client = _httpClientFactory.CreateClient();

            var json = JsonConvert.SerializeObject(dto);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "https://localhost:7212/api/Auth/ForgotPassword",
                content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Email not found.";
                return View(dto);
            }

            TempData["Email"] = dto.Email;

            return RedirectToAction(nameof(ResetPassword));
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            var model = new ResetPasswordDto();

            if (TempData["Email"] != null)
            {
                model.Email = TempData["Email"].ToString();
                TempData.Keep("Email");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View(dto);
            }

            var client = _httpClientFactory.CreateClient();

            var apiDto = new
            {
                Email = dto.Email,
                OTP = dto.OTP,
                NewPassword = dto.NewPassword
            };

            var json = JsonConvert.SerializeObject(apiDto);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "https://localhost:7212/api/Auth/ResetPassword",
                content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid OTP or OTP expired.";
                return View(dto);
            }

            TempData["Success"] = "Password changed successfully.";

            return RedirectToAction(nameof(Login));
        }

        // =========================
        // LOGOUT
        // =========================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}