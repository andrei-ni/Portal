using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Search;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Portal.Data;
using Portal.Models;
using Portal.Services;
using Portal.ViewModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        public readonly INotyfService _notifyService;
        private readonly IConfiguration _config;
        private readonly IStringLocalizer _localizer;

        public HomeController(ILogger<HomeController> logger, DataContext context, INotyfService notifyService, IEmailService emailService, IConfiguration config,
            IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _context = context;
            _notifyService = notifyService;
            _emailService = emailService;
            _config = config;
            _localizer = localizer;
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        // In case of ajax redirect add "Dictionary<bool, string> notification" here
        public async Task<IActionResult> Index()
        {


            var dbCategories = await _context.Categories.Include(x => x.ContentDictionary).ToListAsync();
            var viewModel = new HomeViewModel()
            {
                Categories = dbCategories
            };
            return View(viewModel);
        }

        public IActionResult SetLanguageIndex(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            var referrer = Request.Headers["Referer"].ToString(); // gets the URL of the page where the request came from
            if (string.IsNullOrEmpty(referrer))
            {
                // If there's no referrer, redirect to the home page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(referrer);
            }
            //return RedirectToAction("Index", "Home");
        }

        public FileContentResult DownloadAppManual()
        {
            string basePath = _config.GetValue<string>("FileStoragePath");
            string fileName = "21303.pdf";
            var filePath = Path.Combine(Path.GetFullPath(basePath), fileName);
            byte[] FileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(FileBytes, "application/pdf");
        }

        public async Task<IActionResult> SearchAdvanced()
        {
            var dbApps = await _context.Applications.Include(x => x.Keywords).Include(y => y.ContentDictionary).ToListAsync();
            var viewModel = new HomeViewModel()
            {
                ApplicationList = dbApps
            };
            return View(viewModel);
        }

        public async Task<IActionResult> SearchApps(string searchText)
        {
            var filteredApps = new List<Application>();
            List<string> appsDetails = new();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = RemoveDiacritics(searchText?.ToLower());
                try
                {
                    var dbApps = await _context.Applications.Include(x => x.Keywords).Include(y => y.ContentDictionary).ToListAsync();

                    if (dbApps != null)
                    {
                        filteredApps = dbApps
                        .Where(app => app.Keywords != null && app.Keywords.Any(y => y.Word?.ToLower() == searchText?.ToLower()) ||
                               (app.ContentDictionary != null && app.ContentDictionary.Any(z =>
                               (z.Name != null && RemoveDiacritics(z.Name.ToLower()).Contains(searchText))
                               || (z.Description != null && RemoveDiacritics(z.Description.ToLower()).Contains(searchText)))))
                        .ToList();
                        foreach (var app in filteredApps)
                        {
                            appsDetails.Add(app.Id.ToString());
                            foreach (var content in app.ContentDictionary)
                            {
                                appsDetails.Add(content?.Name?.ToLower());
                                appsDetails.Add(content?.Description?.ToLower());
                            }
                        }
                    }

                }
                catch (Exception ex) { }

                if (filteredApps == null || filteredApps.Count == 0)
                {
                    return Json("Not found");
                }
            }
            return Json(appsDetails);
        }
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
