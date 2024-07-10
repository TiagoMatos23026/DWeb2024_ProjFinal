using DWebProjFinal.Data;
using DWebProjFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Diagnostics;

namespace DWebProjFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ApplicationDbContext context,
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager
            )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        /// Método para ir buscar as Páginas
        /// e os seus respetivos Utentes
        /// e mostrá-los na Homepage
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var paginas = _context.Paginas.
                Include(u => u.Utente)
                .ToArray();

            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);


            return View(paginas);
        }

        /// <summary>
        /// Método para ir buscar as declarações de privacidade
        /// </summary>
        /// <returns></returns>
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
