using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using Portal.Extensions;
using Portal.Middleware;
using Portal.Services;
using WinAuth.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddScoped<RequestLocalizationCookiesMiddleware>();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
builder.Services.AddScoped<IEmailService, EmailService>();

// This is used to get the images from server and convert them to base64 for the pdf
var handler = new HttpClientHandler()
{
    UseDefaultCredentials = false,
    Credentials = System.Net.CredentialCache.DefaultCredentials,
    AllowAutoRedirect = true
};


builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetService<IConfiguration>();
    var baseAddress = configuration["BaseAddress"] ?? "http://localhost:3781/";

    return new HttpClient(handler)
    {
        BaseAddress = new Uri(baseAddress)
    };
});
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomLeft; });

var app = builder.Build();

var supportedCultures = new[] { "ro", "en", "ja" };
var localizationOptions = new RequestLocalizationOptions()
{
    ApplyCurrentCultureToResponseHeaders = true,
    FallBackToParentCultures = true,
    FallBackToParentUICultures = true,
    RequestCultureProviders = new List<IRequestCultureProvider>()
    {
        // remove the AcceptLanguageHeaderRequestCultureProvider because
        // it can override UI elements that allow our users to pick their culture.
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    }
}
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);


app.UseRequestLocalization(localizationOptions);
app.UseRequestLocalizationCookies();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseNotyf();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseStatusCodePagesWithRedirects("/Error/NotFound"); // Redirect to 404 custom page

app.Run();