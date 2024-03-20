using KFU.Common;
using KFU.ScopedService;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure all application services
AddScopedService.RegisterServices(builder.Services , builder.Configuration);

builder.Services.AddAntiforgery(options =>
{
options.SuppressXFrameOptionsHeader = true;
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
options.Cookie.HttpOnly = true;
options.Cookie.Name = "del-portal";
});

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("del-portalSettings.json")
                            .Build();
ApplicationStaticContentSetting.Init(configuration);
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDataProtection()
    .SetApplicationName($"del-portal.kfu.edu.sa-{builder.Environment.EnvironmentName}")
    .PersistKeysToFileSystem(new DirectoryInfo(@ApplicationStaticContentSetting.Config.PathKeys))
    .SetDefaultKeyLifetime(TimeSpan.FromDays(7));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//AddScopedService.RegisterAppUsings(app);
app.Run();



//using KFU.ScopedService;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.CookiePolicy;
//using Microsoft.AspNetCore.Http.Extensions;
//using KFU.Common;
//using Portal.Ui.Arabic;
//using Microsoft.AspNetCore.DataProtection;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//builder.Services.AddAntiforgery(options =>
//{
//    options.SuppressXFrameOptionsHeader = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    options.Cookie.HttpOnly = true;
//    options.Cookie.Name = "del-portal";

//});
//builder.Services.AddMvc();
//IConfiguration configuration = new ConfigurationBuilder()
//                            .AddJsonFile("del-portalSettings.json")
//                            .Build();
//ApplicationStaticContentSetting.Init(configuration);
//builder.Services.AddDistributedMemoryCache();
//AddScopedService.RegisterServices(builder.Services);

//builder.Services.AddDataProtection()
//    .SetApplicationName($"del-portal.kfu.edu.sa-{builder.Environment.EnvironmentName}")
//    .PersistKeysToFileSystem(new DirectoryInfo(@ApplicationStaticContentSetting.Config.PathKeys))
//    .SetDefaultKeyLifetime(TimeSpan.FromDays(7));

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromDays(7);
//});

//builder.Services.AddAuthorizationBuilder()
//    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build());

//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.Secure = CookieSecurePolicy.Always;
//    options.HttpOnly = HttpOnlyPolicy.Always;
//    options.CheckConsentNeeded = context => true;
//    options.MinimumSameSitePolicy = SameSiteMode.None;

//});
//builder.Services.AddScoped<IPortalApplicationCore, PortalApplicationCore>();
////builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
////    .AddCookie(options =>
////    {
////        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
////        options.Cookie.SameSite = SameSiteMode.None;
////        options.ExpireTimeSpan = TimeSpan.FromDays(7);
////        options.SlidingExpiration = true;
////        options.Cookie.HttpOnly = true;
////        options.AccessDeniedPath = "/Forbidden/";
////        options.LoginPath = "/Login/";
////        options.LogoutPath = "/Logout/";
////    });


//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//app.Use(async (context, next) =>
//{
//    var url = context.Request.GetEncodedUrl();
//    context.Response.Headers.Append("X-Frame-Options", "DENY");
//    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
//    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
//    context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");
//    context.Response.Headers.Remove("Server");
//    context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000");
//    context.Response.Headers.Append("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
//    //context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
//    //context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self';");

//    await next();
//    if (context.Response.StatusCode == 404)
//    {
//        context.Request.Path = "/not-found";

//        await next();
//    }
//    if (context.Response.StatusCode == 500)
//    {
//        context.Request.Path = "/not-found";

//        await next();
//    }
//});
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapDefaultControllerRoute();
//app.MapControllerRoute(name: "Forbidden",
//                pattern: "Forbidden",
//                defaults: new { controller = "Home", action = "Forbidden" });
//app.MapControllerRoute(name: "not-found",
//                pattern: "not-found",
//                defaults: new { controller = "Home", action = " Not_found" });
//app.MapControllerRoute(
//    name: "default",
//    //pattern: "{controller=Account}/{action=Index}/{id}");
//    pattern: "{controller=Exams}/{action=GetStudentExams}/{id}");

//app.Run();