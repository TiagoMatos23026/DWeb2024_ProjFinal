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
        /// Objeto que contém os dados do servidor
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UtentesController(
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Utentes
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utentes.ToListAsync());
        }

        // GET: Utentes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utentes = await _context.Utentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utentes == null)
            {
                return NotFound();
            }

            return View(utentes);
        }

        // GET: Utentes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utentes/Create
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
                    ModelState.AddModelError("", "Nenhuma imagem foi fornecida");   //mensagem de erro
                    return View(utente);   //volta para o início com os dados já preenchidos
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
                    //encolher a imagem a um tamanho apropriado
                    //procurar package no nuget que trate disso

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

            var utentes = await _context.Utentes.FindAsync(id);
            if (utentes == null)
            {
                return NotFound();
            }
            return View(utentes);
        }

        // PUT: Utentes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Icon,Email,Telemovel,Password,dataNasc,Biografia")] Utentes utentes)
        {
            if (id != utentes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utentes);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtentesExists(utentes.Id))
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
            return View(utentes);
        }

        // GET: Utentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utentes = await _context.Utentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utentes == null)
            {
                return NotFound();
            }

            return View(utentes);
        }

        // POST: Utentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utentes = await _context.Utentes.FindAsync(id);
            if (utentes != null)
            {
                _context.Utentes.Remove(utentes);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UtentesExists(int id)
        {
            return _context.Utentes.Any(e => e.Id == id);
        }
    }
}
