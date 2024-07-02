using DWebProjFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Diagnostics;

namespace DWebProjFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager
            )
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
