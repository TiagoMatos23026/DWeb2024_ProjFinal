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

namespace DWebProjFinal.Controllers
{
    [Authorize]
    public class PaginasController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<IdentityUser> _userManager;

        public PaginasController(

            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: Paginas
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Paginas.Include(p => p.Utente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Paginas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paginas = await _context.Paginas
                .Include(p => p.Utente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paginas == null)
            {
                return NotFound();
            }

            return View(paginas);
        }

        // GET: Paginas/Create
        public IActionResult Create()
        {
            ViewData["UtenteFK"] = new SelectList(_context.Utentes.OrderBy(c => c.Nome), "Id", "Nome");
            return View();
        }

        // POST: Paginas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descricao,Conteudo,Dificuldade")] Paginas pagina, IFormFile ImgThumbnail)
        {
            var userID = _userManager.GetUserId(User);
            var utente = _context.Utentes.Include(x => x.ListaPaginas).FirstOrDefault(u => u.UserID == userID);

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
            utente.ListaPaginas.Add(pagina);

            _context.Update(utente);

            _context.Add(pagina);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Paginas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paginas = await _context.Paginas.FindAsync(id);
            if (paginas == null)
            {
                return NotFound();
            }
            ViewData["UtenteFK"] = new SelectList(_context.Utentes, "Id", "Email", paginas.UtenteFK);
            return View(paginas);
        }

        // POST: Paginas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descricao,Dificuldade,Conteudo,Thumbnail,UtenteFK")] Paginas paginas)
        {
            if (id != paginas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paginas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaginasExists(paginas.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtenteFK"] = new SelectList(_context.Utentes, "Id", "Email", paginas.UtenteFK);
            return View(paginas);
        }

        // GET: Paginas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paginas = await _context.Paginas
                .Include(p => p.Utente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paginas == null)
            {
                return NotFound();
            }

            return View(paginas);
        }

        // POST: Paginas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paginas = await _context.Paginas.FindAsync(id);
            if (paginas != null)
            {
                _context.Paginas.Remove(paginas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaginasExists(int id)
        {
            return _context.Paginas.Any(e => e.Id == id);
        }
    }
}
