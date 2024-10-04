using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Portal.Data;
using Portal.Models;
using Portal.ViewModel;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notifyService;
        private readonly IStringLocalizer _localizer;

        public ApplicationController(DataContext context, INotyfService notifyService, IStringLocalizer<CategoryController> localizer)
        {
            _context = context;
            _notifyService = notifyService;
            _localizer = localizer;

        }
        public async Task<IActionResult> Index()
        {
                var dbApps = await _context.Applications.Include(x => x.ContentDictionary).ToListAsync();
                if (dbApps != null)
                {
                    var viewModel = new ApplicationViewModel()
                    {
                        Applications = dbApps
                    };
                    return View(viewModel);
                }
            
            return RedirectToRoute(new { controller = "Error", action = "Error", statusCode = 404 });
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> AddApplicationIndex()
        {
            var viewModel = new ApplicationViewModel();
            var dbCategories = await _context.Categories.Include(x => x.ContentDictionary).ToListAsync();
            if (dbCategories != null)
            {
                viewModel = new ApplicationViewModel()
                {
                    DbCategoryList = dbCategories,
                };
                return View(viewModel);
            }
            return RedirectToRoute(new { controller = "Error", action = "Error", statusCode = 404 });
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> EditApplicationIndex(int id)
        {
            Application dbApplication = await _context.Applications.Include(x => x.ContentDictionary)
                .Include(c => c.Categories).Include(k => k.Keywords).FirstOrDefaultAsync(x => x.Id == id);
            var dbCategories = await _context.Categories.Include(x => x.ContentDictionary).ToListAsync();
            if (dbApplication == null)
            {
                return NotFound();
            }
            var viewModel = new ApplicationViewModel()
            {
                Application = dbApplication,
                CategoryList = dbCategories
            };
            return View(viewModel);
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> SaveApplication(Dictionary<string, ApplicationEntityDictionary> content, List<string> selectedOptions,
            string icon, string linkDev, string linkApp, string linkManual, string[] kwords)
        {
            if (content.Count == 0 || icon == null || string.IsNullOrEmpty(linkDev) || string.IsNullOrEmpty(linkApp) || string.IsNullOrEmpty(linkManual))
            {
                _notifyService.Information(_localizer["All fields are required"]);
            }
            else
            {
                var dbCategories = await _context.Categories.ToListAsync();
                List<ApplicationEntityDictionary> appEntityDictionary = new List<ApplicationEntityDictionary>();
                foreach (var item in content)
                {
                    appEntityDictionary.Add(item.Value);
                }
                var newApp = new Application();
                newApp.IconLocation = icon;
                newApp.ContentDictionary = appEntityDictionary;
                newApp.AppLink = linkApp;
                newApp.DevLink = linkDev;
                newApp.ManualLink = linkManual;
                List<Keyword>? Keywords = new();

                foreach (var word in kwords)
                {
                    Keywords.Add(new Keyword { Word = word });
                }

                newApp.Keywords = Keywords;
                List<Category>? categories = new();

                if (selectedOptions.Count != 0 && dbCategories != null)
                {
                    foreach (var cat in dbCategories)
                    {
                        foreach (var option in selectedOptions)
                        {
                            if (cat.Id.ToString() == option)
                            {
                                categories.Add(cat);
                            }
                        }
                    }

                }
                newApp.Categories = categories;

                await _context.Applications.AddAsync(newApp);
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
        }


        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> UpdateApplication(Dictionary<string, ApplicationEntityDictionary> content, string icon, int applicationId,
            int[] addCategoriesIds, int[] removeCategoriesIds, int[] removeKeywordIds, string[] addKeywords, string devLink, string appLink, string manualLink)
        {

            if (icon == null)
            {
                _notifyService.Information(_localizer["All fields are required"]);
            }
            else
            {
                var dbApplication = await _context.Applications.Include(x => x.ContentDictionary).Include(c => c.Categories).Include(k => k.Keywords)
                    .FirstOrDefaultAsync(x => x.Id == applicationId);
                if (dbApplication != null)
                {
                    dbApplication.IconLocation = null;
                    if (dbApplication.ContentDictionary != null) dbApplication.ContentDictionary.Clear();

                    List<ApplicationEntityDictionary> appEntityDictionary = new List<ApplicationEntityDictionary>();
                    foreach (var item in content)
                    {
                        appEntityDictionary.Add(item.Value);
                    }

                    appEntityDictionary.FirstOrDefault(x => x.LanguageCode == "RO").Description = appEntityDictionary.FirstOrDefault(x => x.LanguageCode == "RO").Description.Trim();
                    //appEntityDictionary.FirstOrDefault(x => x.LanguageCode == "RO").Description.TrimStart(' ');
                    //appEntityDictionary.FirstOrDefault(x => x.LanguageCode == "RO").Description.TrimEnd(' ');

                    dbApplication.IconLocation = icon;
                    dbApplication.ContentDictionary = appEntityDictionary;

                    if (addCategoriesIds.Count() > 0 || removeCategoriesIds.Count() > 0)
                    {
                        if (addCategoriesIds.Count() > 0)
                        {
                            List<Category> newCategories = await _context.Categories.Where(x => addCategoriesIds.Contains(x.Id)).ToListAsync();
                            if (newCategories != null)
                            {
                                foreach (var cat in newCategories)
                                {
                                    dbApplication.Categories.Add(cat);
                                }
                            }
                        }

                        if (removeCategoriesIds.Count() > 0)
                        {
                            List<Category> removeCategories = await _context.Categories.Where(x => removeCategoriesIds.Contains(x.Id)).ToListAsync();
                            if (removeCategories != null)
                            {
                                foreach (var cat in removeCategories)
                                {
                                    dbApplication?.Categories?.Remove(cat);
                                }
                            }
                        }
                    }

                    if (removeKeywordIds.Count() > 0)
                    {
                        List<Keyword> removeKeywords = await _context.Keywords.Where(x => removeKeywordIds.Contains(x.Id)).ToListAsync();
                        if (removeKeywords.Count() > 0)
                        {
                            foreach (var word in removeKeywords)
                            {
                                dbApplication?.Keywords?.Remove(word);
                                _context.Keywords.Remove(word);
                            }
                        }
                    }
                    List<Keyword>? Keywords = new();

                    if (addKeywords.Count() > 0)
                    {
                        foreach (var word in addKeywords)
                        {
                            if (!dbApplication.Keywords.Any(x => x.Word == word))
                            {
                                Keywords.Add(new Keyword { Word = word });
                            }
                        }
                    }
                    dbApplication.Keywords.AddRange(Keywords);

                    dbApplication.DevLink = devLink;
                    dbApplication.AppLink = appLink;
                    dbApplication.ManualLink = manualLink;

                    _context.Applications.Update(dbApplication);
                    await _context.SaveChangesAsync();
                    return Json("Success");
                }

            }
            return Json("");
        }

        [Authorize(Roles = "EDIT")] // "Roles" is a claim
        public async Task<IActionResult> DeleteApplication(int applicationId)
        {
            var dbApplication = await _context.Applications.Include(x => x.ContentDictionary).Include(x => x.Keywords).FirstOrDefaultAsync(x => x.Id == applicationId);

            if (dbApplication != null)
            {
                _context.Applications.Remove(dbApplication);
                await _context.SaveChangesAsync();
                return Json("Success");

            }
            return Json("");
        }

        
    }
}
