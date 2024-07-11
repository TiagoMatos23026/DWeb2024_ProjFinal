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
using DWebProjFinal.Areas.Identity.Pages.Account;
using DWebProjFinal.Data.Migrations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace DWebProjFinal.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtentesAPIController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly UtentesController _utentesController;
        private readonly TokenGenerateController _tokenGenerateController;

        public UtentesAPIController(
          UtentesController utentesController,
          UserManager<IdentityUser> userManager,
          SignInManager<IdentityUser> signInManager,
          ApplicationDbContext context,
          TokenGenerateController tokenGenerateController)

        {
            _tokenGenerateController = tokenGenerateController;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _utentesController = utentesController;
        }

        /// <summary>
        /// Modelo para Registo
        /// </summary>
        public class RegisterModel
        {
            public Utentes utente { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "As passwords não coincidem.")]
            public string ConfirmPassword { get; set; }
        }

        /// <summary>
        /// Modelo para Login
        /// </summary>
        public class LoginModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        /// <summary>
        /// Método para Buscar a lista de Utentes
        /// </summary>
        /// <returns></returns>
        // GET: api/UtentesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utentes>>> GetUtentes()
        {
            return await _context.Utentes.ToListAsync();
        }

        /// <summary>
        /// Método para Buscar um Utente em específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/UtentesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utentes>> GetUtentes(int id)
        {
            var utentes = await _context.Utentes.FindAsync(id);

            if (utentes == null)
            {
                return NotFound();
            }

            return utentes;
        }

        /// <summary>
        /// Método para Editar Utentes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="utentes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Método para criar um novo Utente
        /// </summary>
        /// <param name="registo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostUtentes([FromForm] RegisterModel registo)
        {
            if (ModelState.IsValid)
            {
                var result = await Register(registo.Email, registo.Password, registo.ConfirmPassword);

                if (result is OkResult)
                {
                    _context.Add(registo.utente);
                    await _context.SaveChangesAsync();

                    // Automatically confirm the email
                    var user = await _userManager.FindByEmailAsync(registo.Email);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

                    if (confirmResult.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Error ao confirmar o email");
                    }
                }
                else
                {
                    return result;
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Método para Registar um novo Login
        /// É chamado ao ser criado um novo Utente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> Register(string Email, string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid && Password == ConfirmPassword)
            {
                var user = new IdentityUser { UserName = Email, Email = Email };
                var result = await _userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Método para efetuar Login com um Utente
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                var token = _tokenGenerateController.GetToken(model.Email, model.Password);

                if (result.Succeeded)
                {
                    return Ok(token);
                }
                if (result.IsLockedOut)
                {
                    return BadRequest("User account locked out.");
                }
                else
                {
                    return BadRequest("Invalid login attempt.");
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Método para Apagar Utente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
