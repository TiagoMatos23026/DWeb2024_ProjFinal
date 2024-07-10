using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Reflection;
using DWebProjFinal.Data.Migrations;

namespace DWebProjFinal.Controllers
{
    [Authorize]
    public class PaginasController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly CategoriasController _categoriasController;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<IdentityUser> _userManager;

        public PaginasController(
            CategoriasController categoriasController,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _categoriasController = categoriasController;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        /// <summary>
        /// Lista de Páginas
        /// (Não está a ser utilizado, retorna NotFound)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Paginas.Include(p => p.Utente);
            return NotFound();
        }

        /// <summary>
        /// Mostra a Página e o seu conteúdo
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

            var paginas = await _context.Paginas
                .Include(p => p.Utente)
                .Include(l => l.ListaCategorias)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paginas == null)
            {
                return NotFound();
            }

            return View(paginas);
        }

        /// <summary>
        /// Método para ir buscar a View para Criar Páginas
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewData["UtenteFK"] = new SelectList(_context.Utentes.OrderBy(c => c.Nome), "Id", "Nome");
            return View();
        }

        /// <summary>
        /// Método para Criar Páginas
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="ImgThumbnail"></param>
        /// <param name="ListaCategorias"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descricao,Conteudo,Dificuldade")] Paginas pagina, IFormFile ImgThumbnail, string[] ListaCategorias)
        {
            var userID = _userManager.GetUserId(User);
            var utente = _context.Utentes.Include(x => x.ListaPaginas)
                .FirstOrDefault(u => u.UserID == userID);

            //guarda o objeto dentro do modelo a ser uploaded
            pagina.Utente = utente;
            pagina.UtenteFK = utente.Id;

            //-----------------------------//
            //Algoritmo para upload de imagem
            //-----------------------------//
            string nomeImagem = "";
            bool haImagem = false;

            // há ficheiro?
            if (ImgThumbnail == null)
            {
                // não há
                // crio msg de erro
                ModelState.AddModelError("",
                   "Deve fornecer uma thumbnail");
                // devolver controlo à View
                return View(pagina);
            }
            else
            {
                // há ficheiro, mas é uma imagem?
                if (!(ImgThumbnail.ContentType == "image/png" ||
                     ImgThumbnail.ContentType == "image/jpeg" ||
                     ImgThumbnail.ContentType == "image/jpg"
                   ))
                {
                    // não
                    // vamos usar uma imagem pre-definida
                    pagina.Thumbnail = "defaultThumbnail.png";
                }
                else
                {
                    // há imagem
                    haImagem = true;
                    // gerar nome imagem
                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString();
                    string extensaoImagem = Path.GetExtension(ImgThumbnail.FileName).ToLowerInvariant();
                    nomeImagem += extensaoImagem;
                    // guardar o nome do ficheiro na BD
                    pagina.Thumbnail = nomeImagem;
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
                await ImgThumbnail.CopyToAsync(stream);

            }
            //--------------//
            //Fim do algoritmo
            //--------------//

            foreach (var Nome in ListaCategorias)
            {
                if (Nome == null || Nome == "")
                {

                }
                else
                {
                    var categoria = _context.Categorias.FirstOrDefault(c => c.Nome == Nome);

                    if (categoria == null) //se a categoria inserida não existir na bd
                    {
                        var newCategoria = new Categorias { Nome = Nome }; //cria uma nova

                        _categoriasController.Create(newCategoria); //chama o método Create de Categorias

                        _context.Categorias.Add(newCategoria); //insere a nova categoria na bd
                        _context.SaveChanges();

                        pagina.ListaCategorias.Add(newCategoria); //adiciona a nova categoria à lista de categorias da página
                    }
                    else
                    {
                        pagina.ListaCategorias.Add(categoria);
                    }
                }
            }

            utente.ListaPaginas.Add(pagina);

            _context.Update(utente);

            _context.Add(pagina);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Método para ir buscar a Página a ser Editada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagina = _context.Paginas.Include(p => p.Utente).FirstOrDefault(c => c.Id == id);
            var userAtual = _userManager.GetUserId(User);

            if (pagina.Utente.UserID != userAtual)
            {
                return BadRequest();
            }

            if (pagina == null)
            {
                return NotFound();
            }
            return View(pagina);
        }

        /// <summary>
        /// Método para Editar Páginas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pagina"></param>
        /// <param name="ImgThumbnail"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descricao,Dificuldade,Conteudo,Thumbnail,UtenteFK")] Paginas pagina, IFormFile? ImgThumbnail)
        {
            if (id != pagina.Id)
            {
                return NotFound();
            }

            var utente = _context.Utentes.FirstOrDefault(u => u.Id == pagina.UtenteFK);

            var paginaUserID = utente.UserID;

            var userAtual = _userManager.GetUserId(User);

            if (userAtual != paginaUserID)
            {
                return NotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    //-----------------------------//
                    //Algoritmo para upload de imagem
                    //-----------------------------//
                    string nomeImagem = "";
                    bool haImagem = false;

                    // há ficheiro?
                    if (ImgThumbnail == null)
                    {
                        
                    }
                    else
                    {
                        // há ficheiro, mas é uma imagem?
                        if (!(ImgThumbnail.ContentType == "image/png" ||
                             ImgThumbnail.ContentType == "image/jpeg" ||
                             ImgThumbnail.ContentType == "image/jpg"
                           ))
                        {
                            // não
                            // vamos usar uma imagem pre-definida
                            pagina.Thumbnail = "defaultThumbnail.png";
                        }
                        else
                        {
                            // há imagem
                            haImagem = true;
                            // gerar nome imagem
                            Guid g = Guid.NewGuid();
                            nomeImagem = g.ToString();
                            string extensaoImagem = Path.GetExtension(ImgThumbnail.FileName).ToLowerInvariant();
                            nomeImagem += extensaoImagem;
                            // guardar o nome do ficheiro na BD
                            pagina.Thumbnail = nomeImagem;
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
                        await ImgThumbnail.CopyToAsync(stream);

                    }
                    //--------------//
                    //Fim do algoritmo
                    //--------------//

                    try
                    {
                        _context.Update(pagina);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PaginasExists(pagina.Id))
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
            }

            return View();
        }

        /// <summary>
        /// Método para ir buscar a Página a ser Apagada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paginas = await _context.Paginas
                .Include(p => p.Utente)
                .FirstOrDefaultAsync(m => m.Id == id);

            var userAtual = _userManager.GetUserId(User);

            if (paginas.Utente.UserID != userAtual)
            {
                return BadRequest();      
            }

            if (paginas == null)
            {
                return NotFound();
            }

            return View(paginas);
        }

        /// <summary>
        /// Método para Apagar Páginas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paginas = await _context.Paginas
                .Include(p => p.Utente)
                .FirstOrDefaultAsync(m => m.Id == id);

            var userAtual = _userManager.GetUserId(User);

            if (paginas.Utente.UserID != userAtual)
            {
                return BadRequest();
            }

            if (paginas != null)
            {
                _context.Paginas.Remove(paginas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Método para saber se a Página existe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool PaginasExists(int id)
        {
            return _context.Paginas.Any(e => e.Id == id);
        }
    }
}
