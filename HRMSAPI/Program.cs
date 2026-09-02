using Helpers;
using HRMSAPI.Data;
using HRMSAPI.Helpers;
using HRMSAPI.Mappings;
using HRMSAPI.Middleware;
using HRMSAPI.Repository;
using HRMSAPI.Repository.Interfaces;
using HRMSAPI.Services;
using HRMSAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =========================
// SERVICES
// =========================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();

// ✅ SWAGGER WITH JWT SUPPORT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer TOKEN'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ✅ DATABASE
builder.Services.AddDbContext<HRMSDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DBCS")));

// ✅ SERVICES
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IEmailService,
    EmailService>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IPerformanceReviewRepository,
    PerformanceReviewRepository>();
builder.Services.AddScoped<
    IPerformanceReviewService,
    PerformanceReviewService>();
builder.Services.AddScoped<
    INotificationRepository,
    NotificationRepository>();
builder.Services.AddScoped<
    INotificationService,
    NotificationService>();
builder.Services.AddScoped<
    IEmployeeOnboardingRepository,
    EmployeeOnboardingRepository>();
builder.Services.AddScoped<
    IEmployeeOnboardingService,
    EmployeeOnboardingService>();
builder.Services.AddScoped<IEmployeeDocumentRepository,
EmployeeDocumentRepository>();

builder.Services.AddScoped<IEmployeeDocumentService,
EmployeeDocumentService>();
builder.Services.AddScoped<IITAssetRepository, ITAssetRepository>();

builder.Services.AddScoped<IITAssetService, ITAssetService>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IShiftService, ShiftService>();

// =========================
// JWT CONFIG
//// =========================
//=====================================================
//GITHUB
var key = builder.Configuration["Jwt:Key"];
builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true);
//==================================================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key))
        };
});

var app = builder.Build();

// =========================
// PIPELINE
// =========================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ GLOBAL EXCEPTION MIDDLEWARE
app.UseMiddleware<ExceptionMiddleware>();

// ✅ REQUEST LOGGING
app.UseMiddleware<RequestLoggingMiddleware>();

// ✅ AUTH
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();