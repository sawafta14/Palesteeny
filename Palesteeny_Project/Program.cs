using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Palesteeny_Project.Models;
using Palesteeny_Project.Services;  // ??????? ??? ????????? ??????

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8); // مدة صلاحية الجلسة
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// ????? DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DbTest;Integrated Security=True");
});

// ????? ????? ???????? ???????? ????? ????? ???????? (Cookies)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // ?????? ???? ??? ????? ??????? ????? ??? ?? ??? ????? ??????
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// ????? ??????? ??????? ?? appsettings.json ?? ?????? ?? ??????
var emailSettings = builder.Configuration.GetSection("EmailSettings");

string host = emailSettings.GetValue<string>("Host")
    ?? throw new Exception("Email Host is not configured in appsettings.json");

int port = emailSettings.GetValue<int?>("Port") ?? 587;

string fromEmail = emailSettings.GetValue<string>("FromEmail")
    ?? throw new Exception("Email FromEmail is not configured in appsettings.json");

string password = emailSettings.GetValue<string>("Password")
    ?? throw new Exception("Email Password is not configured in appsettings.json");

// ????? ???? ????? ???????
builder.Services.AddSingleton<IEmailSender>(new SmtpEmailSender(host, port, fromEmail, password));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,            // يسمح بتوزيع الملفات حتى لو نوعها غير معروف
    DefaultContentType = "application/octet-stream"
});



app.UseRouting();
app.UseSession();

// ????? ???????? ??? ???????
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
