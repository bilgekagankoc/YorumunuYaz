using Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MvcWebUI.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Hesaplar/Giris";
        config.AccessDeniedPath = "/Hesaplar/YetkisizIslem";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        config.SlidingExpiration = true;
    });
#endregion
#region Session
builder.Services.AddSession(config =>
{
config.IdleTimeout = TimeSpan.FromMinutes(30); 
});
#endregion
#region DI
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IHesapService, HesapService>();
builder.Services.AddScoped<IKategoriService, KategoriService>();
builder.Services.AddScoped<IYorumService, YorumService>();
builder.Services.AddScoped<IYorumCevapService, YorumCevapService>();
#endregion

IConfigurationSection section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings()); // AppSettings class'ýný MVC projemizde Settings klasörü altýnda appsettings.json'daki AppSettings section'ý ile ayný yapýda oluþturuyoruz.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
#region Authentication
app.UseAuthentication(); 
#endregion
app.UseAuthorization();

#region Session

app.UseSession();
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
