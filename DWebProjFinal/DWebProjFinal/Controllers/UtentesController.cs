using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Identity;
using Humanizer.Localisation;
using ImageMagick;

namespace DWebProjFinal.Controllers
{
    [Authorize]
    public class UtentesController : Controller
    {
        /// <summary>
        /// Referência à base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Referência ao Identity para controlo de logins e logouts
        /// </summary>
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Referência ao Identity para controlo do userLogin
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Objeto que contém os dados do servidor
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        /// <param name="webHostEnvironment"></param>
        public UtentesController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Método para ir buscar a lista de Utentes
        /// Não está a ser utilizado, retorna NotFound
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return NotFound();

        }

        /// <summary>
        /// Método para ir buscar um Utente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utentes = await _context.Utentes.Include(u => u.ListaPaginas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (utentes == null)
            {
                return NotFound();
            }

            return View(utentes);
        }

        /// <summary>
        /// Método para ir buscar o Utente que está logged in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DetailsByUserLogin(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userID = _userManager.GetUserId(User);

            if (userID != id)
            {
                return NotFound();
            }

            var utente = await _context.Utentes.Include(u => u.ListaPaginas)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (utente == null)
            {
                return NotFound();
            }

            return View(utente);
        }

        /// <summary>
        /// Método para chamar a View para Criar Utentes
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utentes/Create
        /// <summary>
        /// Método para criar um novo Utente
        /// Este método apenas é chamado dentro do método Register, 
        /// pois apenas são criados novos Utentes quando é registado um novo utilizador
        /// </summary>
        /// <param name="utente"></param>
        /// <param name="IconFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Telemovel,dataNasc,Biografia")] Utentes utente, IFormFile IconFile)
        {
            if (ModelState.IsValid)
            {
                string nomeImagem = "";
                bool haImagem = false;

                if (IconFile == null)   //se não houver ficheiro
                {
                    utente.Icon = "defaultIcon.png";
                }
                else   //se há ficheiro
                {
                    if (!(IconFile.ContentType == "image/png" ||
                        IconFile.ContentType == "image/jpeg" ||
                        IconFile.ContentType == "image/jpg"))   //se o ficheiro não for do tipo imagem
                    {
                        utente.Icon = "defaultIcon.png";   //usa-se uma imagem pre definida
                    }
                    else   //se for imagem
                    {
                        haImagem = true;
                        Guid g = Guid.NewGuid();
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(IconFile.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        utente.Icon = nomeImagem;   //guarda o nome da imagem na BD
                    }
                }

                _context.Add(utente);   //adiciona os dados recebidos ao objeto
                await _context.SaveChangesAsync();   //faz o commit

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

                return RedirectToAction(nameof(Index));   //redireciona para o início
            }
            return View(utente);   //se o modelo de dados não for válido, algo correu mal e volta para o início
        }

        // GET: Utentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes.FindAsync(id);
            //var userLogin = await _userManager.FindByIdAsync(utente.UserID);

            var userAtual = _userManager.GetUserId(User);

            if (userAtual != utente.UserID)
            {
                return BadRequest();
            }

            if (utente == null)
            {
                return NotFound();
            }
            return View(utente);
        }

        // PUT: Utentes/Edit/5
        /// <summary>
        /// Método para Editar Utente
        /// Recebe um id, o utente cujo id foi dado e o UserLogin cujo id coincide com UserId em utente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="utente"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Telemovel,dataNasc,Biografia,UserID,Icon")] Utentes utente, IFormFile? IconFile)
        {
            if (id != utente.Id)
            {
                return NotFound();
            }

            var userAtual = _userManager.GetUserId(User);

            if (userAtual != utente.UserID)
            {
                return BadRequest();
            }

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
                    utente.Icon = "defaultIcon.png";
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
                        utente.Icon = "defaultIcon.png";
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

                try
                {
                    //await _userManager.ChangePasswordAsync(userLogin, currentPassword, newPassword);
                    _context.Update(utente);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    if (!UtentesExists(utente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");

            }
            return View(utente);
        }

        /// <summary>
        /// Método para saber se o Utente que pediu a mudança da password
        /// existe e é o dono do perfil
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ChangePassword(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes
                .FirstOrDefaultAsync(m => m.Id == id);
            var userLogin = await _context.Utentes
                .FirstOrDefaultAsync(m => m.Id == id);

            var userAtual = _userManager.GetUserId(User);


            if (utente.UserID != userAtual)
            {
                return BadRequest();
            }

            if (utente == null || userLogin == null)
            {
                return BadRequest();
            }

            return View(utente);
        }

        /// <summary>
        /// Método para Editar a password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(int id, string currentPassword, string newPassword)
        {
            var utente = await _context.Utentes
                .FirstOrDefaultAsync(m => m.Id == id);

            var userAtual = _userManager.GetUserId(User);

            var user = await _userManager.GetUserAsync(User);

            if (utente.UserID != userAtual)
            {
                return BadRequest();
            }
            try
            {
                await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                return RedirectToAction("DetailsByUserLogin", "Utentes", new { id = userAtual });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }


        /// <summary>
        /// Método para apagar o Utente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes
                .FirstOrDefaultAsync(m => m.Id == id);

            var userLogin = await _userManager
                .FindByIdAsync(utente.UserID);

            var userAtual = _userManager.GetUserId(User);

            if (utente == null || userLogin == null || userAtual != utente.UserID)
            {
                return BadRequest();
            }

            return View(utente);
        }

        // POST: Utentes/Delete/5
        /// <summary>
        /// Apaga o Utente e o respetivo UserLogin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utente = await _context.Utentes
                .FirstOrDefaultAsync(m => m.Id == id);

            var userLogin = await _userManager
                .FindByIdAsync(utente.UserID);

            var userAtual = _userManager.GetUserId(User);

            if (utente == null || userLogin == null || userAtual != utente.UserID)
            {
                return BadRequest();
            }

            if (utente != null)
            {
                await _signInManager.SignOutAsync();
                await _userManager.DeleteAsync(userLogin);
                _context.Utentes.Remove(utente);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        private bool UtentesExists(int id)
        {
            return _context.Utentes.Any(e => e.Id == id);
        }
    }
}
