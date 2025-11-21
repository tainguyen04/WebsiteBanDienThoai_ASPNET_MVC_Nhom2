
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Helpers;
using QLCHBanDienThoaiMoi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Thay thế toàn bộ AddScoped bằng:
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(classes => classes.InNamespaces("QLCHBanDienThoaiMoi.Services"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<SessionHelper>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>

{
    options.IdleTimeout = TimeSpan.FromDays(3); // 3 ngày
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
