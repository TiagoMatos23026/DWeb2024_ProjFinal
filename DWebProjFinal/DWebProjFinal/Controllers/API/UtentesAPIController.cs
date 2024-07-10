using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DWebProjFinal.Controllers.API.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtentesAPIController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ILogger<UtentesAPIController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly UtentesController _utenteController;
        private readonly ApplicationDbContext _context;

        public UtentesAPIController(
            UtentesController utenteController,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<UtentesAPIController> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _utenteController = utenteController;
        }

        // GET: api/UtentesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utentes>>> GetUtentes()
        {
            return await _context.Utentes.ToListAsync();
        }

        // GET: api/UtentesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utentes>> GetUtentes(int id)
        {
            var utentes = await _context.Utentes
                .Include(u => u.ListaPaginas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (utentes == null)
            {
                return NotFound();
            }

            return utentes;
        }

        // PUT: api/UtentesAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtentes(int id, Utentes utentes)
        {
            if (id != utentes.Id)
            {
                return BadRequest();
            }

            _context.Entry(utentes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtentesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        public async Task<ActionResult<Utentes>> Register(string Email, string Password, string ConfirmPassword, Utentes utentes, IFormFile IconFile)
        {
            if (ModelState.IsValid) //validacao
            {

                var userLogin = _userManager.CreateAsync();

                await _userStore.SetUserNameAsync(userLogin, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(userLogin, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(userLogin, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilizador criado com sucesso!");

                    try
                    {
                        Input.utente.UserID = userLogin.Id;
                        await _utenteController.Create(Input.utente, Input.IconFile); //chamar UtentesContoller para criação de um objeto Utente
                    }
                    catch (Exception ex)
                    {
                        await _userManager.DeleteAsync(userLogin);
                        await _context.SaveChangesAsync();
                        Url.Content("~/");
                    }

                    var userId = await _userManager.GetUserIdAsync(userLogin);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(userLogin);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Por favor, confirme o seu email <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(userLogin, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }



                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }






        // POST: api/UtentesAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utentes>> PostUtentes(string Email, string Password, string ConfirmPassword, Utentes utentes, IFormFile IconFile)
        {


            _context.Utentes.Add(utentes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtentes", new { id = utentes.Id }, utentes);
        }



        // DELETE: api/UtentesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtentes(int id)
        {
            var utentes = await _context.Utentes.FindAsync(id);
            if (utentes == null)
            {
                return NotFound();
            }

            _context.Utentes.Remove(utentes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtentesExists(int id)
        {
            return _context.Utentes.Any(e => e.Id == id);
        }
    }
}
