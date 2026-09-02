using HRMSWEB.Services;
using HRMSWEB.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// ==========================
// SERVICES
// ==========================

// MVC
builder.Services.AddControllersWithViews();

// ✅ SESSION
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});

// ✅ HTTP CLIENT
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// ==========================
// PIPELINE
// ==========================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

// ✅ SESSION
app.UseSession();

app.UseAuthorization();

// DEFAULT ROUTE
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();