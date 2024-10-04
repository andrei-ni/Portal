using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using Portal.ViewModel;
using System.Diagnostics;

namespace PhonebookManager.Controllers
{
    public class ErrorController : Controller
    {
        //public IActionResult NotFound()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error/{statusCode}")]
        public IActionResult Error(int? statusCode)
        {
            var errorMsg = new ErrorMessage()
            {
                Title = statusCode.ToString(),
                Message = "Ceva nu a mers bine"
            };
            switch (statusCode.ToString())
            {
                case "401":
                    errorMsg.Message = "Este necesară autorizarea";
                    break;
                case "403":
                    errorMsg.Message = "Accesul restricționat";
                    break;
                case "404":
                    errorMsg.Message = "Pagina nu a fost găsită";
                    break;
                case "405":
                    errorMsg.Message = "Accesul restricționat";
                    break;
                case "500":
                    errorMsg.Message = "Eroare internă a serverului";
                    break;
                default:
                    break;
            }
            return View("~/Views/Shared/CustomError.cshtml", errorMsg);
        }

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
