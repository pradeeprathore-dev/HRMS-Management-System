using Entites.Model;
using HRMSAPI.Data;
using HRMSAPI.DTOs;
using HRMSAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Helpers;
using Microsoft.AspNetCore.Authorization;

namespace HRMSAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HRMSDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMemoryCache _memoryCache;

        private readonly IEmailService _emailService;

        public AuthController(
     HRMSDbContext context,
     ITokenService tokenService,
     IMemoryCache memoryCache,
     IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _memoryCache = memoryCache;
            _emailService = emailService;
        }

        // ✅ REGISTER
        [Authorize(Roles = "1")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // ✅ Username Check
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Username already exists"
                });
            }

            // Employee already linked to a user?
            var existingEmployeeUser = await _context.Users
                .FirstOrDefaultAsync(x => x.EmployeeId == dto.EmployeeId);

            if (existingEmployeeUser != null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "This employee already has a user account"
                });
            }

            var user = new User
            {
                Username = dto.Username,

                // ✅ HASH PASSWORD
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                EmployeeId = dto.EmployeeId,
                RoleId = 2
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "User Registered Successfully"
            });
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // ✅ Find User
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid Username"
                });
            }

            // ✅ Verify Password
            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Invalid Password"
                });
            }

            // ✅ Generate Token
            var token = _tokenService.GenerateToken(user);

            return Ok(new
            {
                Success = true,
                Message = "Login Successful",
                Token = token,
                EmployeeId = user.EmployeeId,
                RoleId = user.RoleId
            });
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(
    ForgotPasswordDto dto)
        {
            var employee =
                await _context.Employees
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (employee == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Email not found."
                });
            }

            var user =
                await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employee.Id);

            if (user == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "User account not found."
                });
            }

            var otp =
                new Random().Next(100000, 999999).ToString();

            _memoryCache.Set(
                dto.Email,
                otp,
                TimeSpan.FromMinutes(5));

            string body = $@"
        <h2>HRMS Password Reset</h2>

        <p>Your OTP is:</p>

        <h1>{otp}</h1>

        <p>This OTP will expire in 5 minutes.</p>";

            await _emailService.SendEmailAsync(
                dto.Email,
                "Password Reset OTP",
                body);

            return Ok(new
            {
                Success = true,
                Message = "OTP sent successfully."
            });
        }
        [HttpPost("VerifyOtp")]
        public IActionResult VerifyOtp(
    VerifyOtpDto dto)
        {
            if (!_memoryCache.TryGetValue(
                dto.Email,
                out string savedOtp))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "OTP expired."
                });
            }

            if (savedOtp != dto.OTP)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid OTP."
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "OTP Verified."
            });
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(
    ResetPasswordDto dto)
        {
            if (!_memoryCache.TryGetValue(
                dto.Email,
                out string savedOtp))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "OTP Expired."
                });
            }

            if (savedOtp != dto.OTP)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid OTP."
                });
            }

            var employee =
                await _context.Employees
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (employee == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Employee not found."
                });
            }

            var user =
                await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employee.Id);

            if (user == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "User not found."
                });
            }

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();

            _memoryCache.Remove(dto.Email);

            return Ok(new
            {
                Success = true,
                Message = "Password Reset Successfully."
            });
        }
    }
}