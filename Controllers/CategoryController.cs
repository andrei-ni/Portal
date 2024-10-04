using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Portal.Data;
using Portal.Models;
using Portal.ViewModel;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace Portal.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notifyService;
        private readonly IStringLocalizer _localizer;

        public CategoryController(DataContext context, INotyfService notifyService, IStringLocalizer<CategoryController> localizer)
        {
            _context = context;
            _notifyService = notifyService;
            _localizer = localizer;

        }
        public async Task<IActionResult> Index(int id)
        {
            var dbCategory = await _context.Categories.Include(x => x.Applications).ThenInclude(x => x.ContentDictionary).FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategory != null)
            {
                var viewModel = new CategoryViewModel()
                {
                    CategoryId = dbCategory.Id,
                    ApplicationList = dbCategory.Applications
                };
                return View(viewModel);
            }
            return RedirectToRoute(new { controller = "Error", action = "Error", statusCode = 404 });
        }



        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> AddCategoryIndex()
        {
            var viewModel = new CategoryViewModel();
            var dbApplications = await _context.Applications.Include(x => x.ContentDictionary).Include(x => x.Categories).ToListAsync();
            if (dbApplications != null)
            {
                viewModel = new CategoryViewModel()
                {
                    DbApplicationList = dbApplications
                };
                return View(viewModel);
            }
            return RedirectToRoute(new { controller = "Error", action = "Error", statusCode = 404 });
        }

        public async Task<IActionResult> UploadIcon(IFormFile file) // NOT USED
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/icons", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Json(new { success = true, message = "File uploaded successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occurred: " + ex.Message });
            }


            //return Json("");
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> EditCategoryIndex(int id)
        {
            Category dbCategory = await _context.Categories.Include(x => x.ContentDictionary).Include(x => x.Applications).FirstOrDefaultAsync(x => x.Id == id);
            var dbApps = await _context.Applications.Include(x => x.ContentDictionary).ToListAsync();
            if (dbCategory == null)
            {
                return NotFound();
            }
            var viewModel = new CategoryViewModel()
            {
                Category = dbCategory,
                ApplicationList = dbApps
            };
            return View(viewModel);
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        [HttpPost]
        public async Task<IActionResult> SaveCategory(Dictionary<string, CategoryEntityDictionary> content, string icon, List<string> selectedOptions)
        {
            if (content.Count == 0 || icon == null)
            {
                _notifyService.Information(_localizer["All fields are required"]);
            }
            else
            {
                var dbApps = await _context.Applications.ToListAsync();
                List<CategoryEntityDictionary> categoryEntityDictionaries = new List<CategoryEntityDictionary>();
                foreach (var item in content)
                {
                    categoryEntityDictionaries.Add(item.Value);
                }

                var newCategory = new Category();
                newCategory.IconLocation = icon;
                newCategory.ContentDictionary = categoryEntityDictionaries;

                List<Application>? applications = new();

                if (selectedOptions.Count != 0 && dbApps != null)
                {
                    foreach (var app in dbApps)
                    {
                        foreach (var option in selectedOptions)
                        {
                            if (app.Id.ToString() == option)
                            {
                                applications.Add(app);
                            }
                        }
                    }

                }
                newCategory.Applications = applications;

                await _context.Categories.AddAsync(newCategory);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return Json("");

            //return RedirectToAction("Index", "Home"); // Doesn't work with ajax
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> UpdateCategory(Dictionary<string, CategoryEntityDictionary> content, string icon, int categoryId,
            int[] addAppsIds, int[] removeAppsIds)
        {

            if (icon == null)
            {
                _notifyService.Information(_localizer["All fields are required"]);
            }
            else
            {
                var dbCategory = await _context.Categories.Include(x => x.ContentDictionary).Include(x => x.Applications).FirstOrDefaultAsync(x => x.Id == categoryId);
                if (dbCategory != null)
                {
                    dbCategory.IconLocation = null;
                    if (dbCategory.ContentDictionary != null) dbCategory.ContentDictionary.Clear();

                    List<CategoryEntityDictionary> categoryEntityDictionaries = new List<CategoryEntityDictionary>();
                    foreach (var item in content)
                    {
                        categoryEntityDictionaries.Add(item.Value);
                    }


                    //categoryEntityDictionaries.FirstOrDefault(x => x.LanguageCode == "RO").Description = categoryEntityDictionaries.FirstOrDefault(x => x.LanguageCode == "RO").Description.Trim();
                    dbCategory.IconLocation = icon;
                    dbCategory.ContentDictionary = categoryEntityDictionaries;


                    List<Application> newApplications = await _context.Applications.Where(x => addAppsIds.Contains(x.Id)).ToListAsync();
                    List<Application> removeApplications = await _context.Applications.Where(x => removeAppsIds.Contains(x.Id)).ToListAsync();
                    if (newApplications.Count() > 0 || removeApplications.Count() > 0)
                    {
                        if (newApplications.Count() > 0)
                        {
                            foreach (var cat in newApplications)
                            {
                                dbCategory.Applications.Add(cat);
                            }
                        }

                        if (removeApplications.Count() > 0)
                        {
                            foreach (var cat in removeApplications)
                            {
                                dbCategory?.Applications?.Remove(cat);
                            }
                        }
                    }


                    _context.Categories.Update(dbCategory);
                    await _context.SaveChangesAsync();
                    return Json("Success");
                }

            }
            return Json("");
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var dbCategory = await _context.Categories.Include(x => x.ContentDictionary).FirstOrDefaultAsync(x => x.Id == categoryId);

            if (dbCategory != null)
            {
                _context.Categories.Remove(dbCategory);
                await _context.SaveChangesAsync();
                return Json("Success");

            }
            return Json("");
        }

        public async Task<IActionResult> SearchKeywords(string searchText, int catId)
        {
            var dbCategory = new Category();
            var filteredApps = new List<Application>();
            List<string> appsDetails = new();
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = RemoveDiacritics(searchText?.ToLower());
                try
                {
                    dbCategory = await _context.Categories.Include(c => c.ContentDictionary)
                        .Include(x => x.Applications).ThenInclude(y => y.Keywords).Include(x => x.Applications).ThenInclude(y => y.ContentDictionary)
                        .FirstOrDefaultAsync(x => x.Id == catId);
                    if (dbCategory != null)
                    {
                        filteredApps = dbCategory.Applications
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
