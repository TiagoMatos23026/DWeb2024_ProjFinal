using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Hosting;

namespace DWebProjFinal.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtentesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UtentesAPIController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
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
            var utentes = await _context.Utentes.FindAsync(id);

            if (utentes == null)
            {
                return NotFound();
            }

            return utentes;
        }

        // PUT: api/UtentesAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtentes(int id, [FromForm] Utentes utentes, IFormFile? iconFile)
        {
            if (id != utentes.Id)
            {
                return BadRequest();
            }

            if (iconFile != null)
            {
                string uniqueFileName = await SaveImage(iconFile);
                utentes.Icon = uniqueFileName;
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

        // POST: api/UtentesAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utentes>> PostUtentes([FromForm] Utentes utente, IFormFile? iconFile)
        {
            string nomeImagem = "";
            bool haImagem = false;



            if (iconFile == null)   //se não houver ficheiro
            {
                ModelState.AddModelError("", "Nenhuma imagem foi fornecida");   //mensagem de erro
            }
            else   //se há ficheiro
            {
                if (!(iconFile.ContentType == "image/png" ||
                    iconFile.ContentType == "image/jpeg" ||
                    iconFile.ContentType == "image/jpg"))   //se o ficheiro não for do tipo imagem
                {
                    utente.Icon = "defaultIcon.png";   //usa-se uma imagem pre definida
                }
                else   //se for imagem
                {
                    haImagem = true;
                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString();
                    string extensaoImagem = Path.GetExtension(iconFile.FileName).ToLowerInvariant();
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
                await iconFile.CopyToAsync(stream);

            }

            return CreatedAtAction("GetUtentes", new { id = utente.Id }, utente);
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

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagens"); //folder onde as imagens ficam guardadas
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName; //nomes das imagens uploaded para o folder
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }
    }
}
