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
using Microsoft.AspNetCore.Hosting;

namespace DWebProjFinal.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class UtentesAPIController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly UtentesController _utentesController;
        private readonly TokenGenerateController _tokenGenerateController;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UtentesAPIController(

          UtentesController utentesController,
          UserManager<IdentityUser> userManager,
          SignInManager<IdentityUser> signInManager,
          ApplicationDbContext context,
          IWebHostEnvironment webHostEnvironment,
          TokenGenerateController tokenGenerateController)

        {
            _webHostEnvironment = webHostEnvironment;
            _tokenGenerateController = tokenGenerateController;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _utentesController = utentesController;
        }

        public class APIModel
        {
            public Utentes utente { get; set; }

            public string token { get; set; }

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

            IFormFile? IconFile { get; set; }
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
        public async Task<ActionResult<Utentes>> GetUtente(int id)
        {
            var utente = await _context.Utentes.FindAsync(id);

            if (utente == null)
            {
                return NotFound();
            }

            return utente;
        }

        [Authorize]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<Utentes>> GetUtenteByEmail(string email)
        {
            var userAtual = _userManager.GetUserId(User);

            var utente = _context.Utentes.FirstOrDefault(m => m.UserID == userAtual);

            if (utente == null)
            {
                return NotFound();
            }

            return utente;
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
        public async Task<IActionResult> PutUtentes(int id, Utentes utentes, string token)
        {
            if (token == null)
            {
                return BadRequest();
            }
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
        public async Task<IActionResult> PostUtentes([FromForm] Utentes utente, [FromForm] IFormFile? IconFile)
        {
            if (ModelState.IsValid)
            {             

                    //-----------------------------//
                    //Algoritmo para upload de imagem
                    //-----------------------------//
                    string nomeImagem = "";
                    bool haImagem = false;

                    // há ficheiro?
                    if (IconFile == null)
                    {
                        utente.Icon = "defaultThumbnail.png";
                    }
                    else
                    {
                        // há ficheiro, mas é uma imagem?
                        if (!(IconFile.ContentType == "image/png" ||
                             IconFile.ContentType == "image/jpeg" ||
                             IconFile.ContentType == "image/jpg"
                           ))
                        {
                            // não
                            // vamos usar uma imagem pre-definida
                            utente.Icon = "defaultThumbnail.png";
                        }
                        else
                        {
                            // há imagem
                            haImagem = true;
                            // gerar nome imagem
                            Guid g = Guid.NewGuid();
                            nomeImagem = g.ToString();
                            string extensaoImagem = Path.GetExtension(IconFile.FileName).ToLowerInvariant();
                            nomeImagem += extensaoImagem;
                            // guardar o nome do ficheiro na BD
                            utente.Icon = nomeImagem;
                        }
                    }

                    //a imagem ao chegar aqui está pronta a ser uploaded
                    if (haImagem)   //apenas segue para aqui se realmente HÁ imagem e é válida
                    {

                        //determinar o local de armazenamento da imagem dentro do disco rígido
                        string localizacaoImagem = _webHostEnvironment.WebRootPath;
                        localizacaoImagem = Path.Combine(localizacaoImagem, "imagens");

                        //será que o local existe?
                        if (!Directory.Exists(localizacaoImagem))   //se não houver local para guardar a imagem...
                        {
                            Directory.CreateDirectory(localizacaoImagem);   //criar um novo local
                        }

                        //existindo local para guardar a imagem, informar o servidor do seu nome
                        //e de onde vai ser guardada
                        string nomeFicheiro = Path.Combine(localizacaoImagem, nomeImagem);

                        //guardar a imagem no disco rígido
                        using var stream = new FileStream(nomeFicheiro, FileMode.Create);
                        await IconFile.CopyToAsync(stream);

                    }
                    //--------------//
                    //Fim do algoritmo
                    //--------------//

                    _context.Add(utente);
                    await _context.SaveChangesAsync();

                    // Automatically confirm the email
                    var user = await _userManager.FindByIdAsync(utente.UserID);
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
