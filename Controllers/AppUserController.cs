using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal.Data;
using Portal.Models;
using Portal.ViewModel;
using PhonebookManager.Models;

namespace Portal.Controllers
{
    [Authorize(Roles = "EDIT")] // "Roles" is a claim
    public class AppUserController : Controller
    {
        private readonly DataContext _context;
        private readonly INotyfService _notifyService;
        public AppUserController(DataContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> dbUsers = new();
            try
            {
                dbUsers = await _context.AppUsers.Include(x => x.Role).OrderBy(x => x.Name).ToListAsync();
            }
            catch (Exception ex)
            {

            }
           


            var viewModel = new AppUsersViewModel()
            {
                UsersList = dbUsers
            };
            return View(viewModel);
        }


        public async Task<JsonResult> AutocompleteSearchUsers(string searchText)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    string[] words = searchText.Split(' ');
                    string firstWord = words[0].ToLower();
                    List<Employee> Employees = new();
                    try
                    {
                        Employees = await _context.Employees.FromSqlRaw("SELECT * FROM All_employees").ToListAsync();
                    }
                    catch { }

                    if (Employees is not null || Employees.Count() != 0)
                    {
                     
                        Employees = Employees.Where(x => x.EmployeeID == firstWord || x.FullName.ToLower().Contains(searchText.ToLower())).ToList();


                        var employeesFiltered = (from user in Employees
                                                 select new
                                                 {
                                                     label = user.EmployeeID + " - " + user.FullName,
                                                     val = user.EmployeeID
                                                 }).ToList();

                        if (employeesFiltered.Count != 0)
                        {
                            return Json(employeesFiltered);

                        }
                        else
                        {
                            return Json("");
                        }
                    }
                }

                return Json("");

            }
            catch (Exception ex)
            {
                return Json("Error-" + ex.Message + " stackTrace-" + ex.StackTrace);
            }
        }

        public async Task<JsonResult> FindUser(string searchText)
        {
            var dbUser = await _context.Employees.FromSqlRaw("SELECT * FROM All_employees").FirstOrDefaultAsync(x => x.EmployeeID == searchText.Replace(" ", ""));
            if (dbUser == null)
            {
                return Json("");
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dbUser);
            return Json(json);
        }

        public async Task<IActionResult> AddUser(AppUser user)
        {
            var viewUser = await _context.Employees.FromSqlRaw("SELECT * FROM All_employees").FirstOrDefaultAsync(x => x.EmployeeID.Replace(" ", "") == user.BadgeNo.Replace(" ", ""));
            var appUsers = await _context.AppUsers.ToListAsync();
            if(viewUser == null)
            {
                _notifyService.Warning("User does not exist");
            }
            else if (appUsers.Any(x=> x.BadgeNo == viewUser.EmployeeID))
            {
                _notifyService.Warning("User exists");
            }
            else if (viewUser.Activity.Replace(" ", "") != "Active")
            {
                _notifyService.Warning("User is inactive");
            }
            else
            {
                AppUser newUser = new AppUser();
                newUser.Role = await _context.AppRoles.FirstOrDefaultAsync(x => x.Role == user.Role.Role);
                newUser.Email = user.Email;
                newUser.Name = user.Name;
                newUser.AdIdentity = user.AdIdentity;
                newUser.BadgeNo = user.BadgeNo;

                await _context.AppUsers.AddAsync(newUser);
                await _context.SaveChangesAsync();

                _notifyService.Success("User added");
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user != null)
            {
                _context.AppUsers.Remove(user);
                await _context.SaveChangesAsync();
                return Json("Success");
            }

            return Json("");
        }
    }
}
