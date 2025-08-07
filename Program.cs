using Microsoft.EntityFrameworkCore;
using Newfactjo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using Newfactjo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
       new CultureInfo("ar"),
       new CultureInfo("en"),
       new CultureInfo("fr"),
       new CultureInfo("de"),
       new CultureInfo("es"),
       new CultureInfo("ru"),
       new CultureInfo("zh"),
       new CultureInfo("tr")
    };

    options.DefaultRequestCulture = new RequestCulture("ar");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// إضافة خدمات MVC مع دعم الترجمة
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// اضافة الادمن
builder.Services.AddSession();

// إضافة DbContext وربط قاعدة البيانات
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PermissionService>();
builder.Services.AddSignalR();


builder.Services.AddHttpContextAccessor();

// **هنا أضفنا رقم المنفذ**
builder.WebHost.UseUrls("http://localhost:7210");

var app = builder.Build();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

// إعداد مسارات التطبيق
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// اضافة ادمن
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<Newfactjo.Hubs.ChatHub>("/chathub");


// 🟢 استدعاء DbInitializer لتعبئة التصنيفات
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    // تأكد من أن قاعدة البيانات موجودة
    context.Database.EnsureCreated();
    // استدعاء المُهيئ
    DbInitializer.Initialize(context);
}

app.Run();
