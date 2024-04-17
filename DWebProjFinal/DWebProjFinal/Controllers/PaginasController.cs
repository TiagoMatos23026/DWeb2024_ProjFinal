using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;

namespace DWebProjFinal.Controllers
{
    public class PaginasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaginasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Paginas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Paginas.Include(p => p.Utente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Paginas/Details/5
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
            ViewData["UtenteFK"] = new SelectList(_context.Utentes, "Id", "Email");
            return View();
        }

        // POST: Paginas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descricao,Dificuldade,UtenteFK")] Paginas pagina, IFormFile ImgThumbnail)
        {
            if (ModelState.IsValid)
            {
                string nomeImagem = "";
                bool haImagem = false;

                // há ficheiro?
                if (ImgThumbnail == null)
                {
                    // não há
                    // crio msg de erro
                    ModelState.AddModelError("",
                       "Deve fornecer um logótipo");
                    // devolver controlo à View
                    return View(pagina);
                }
                else
                {
                    // há ficheiro, mas é uma imagem?
                    if (!(ImgThumbnail.ContentType == "image/png" ||
                         ImgThumbnail.ContentType == "image/jpeg"
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
                _context.Add(pagina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtenteFK"] = new SelectList(_context.Utentes, "Id", "Email", pagina.UtenteFK);
            return View(pagina);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descricao,Dificuldade,Thumbnail,UtenteFK")] Paginas paginas)
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
