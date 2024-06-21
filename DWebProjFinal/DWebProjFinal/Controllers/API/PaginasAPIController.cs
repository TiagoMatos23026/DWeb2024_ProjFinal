using System;
using System.Collections.Generic;
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
    public class PaginasAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PaginasAPIController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        // GET: api/PaginasAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paginas>>> GetPaginas()
        {
            return await _context.Paginas.ToListAsync();
        }

        // GET: api/PaginasAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paginas>> GetPaginas(int id)
        {
            var paginas = await _context.Paginas.FindAsync(id);

            if (paginas == null)
            {
                return NotFound();
            }

            return paginas;
        }

        // PUT: api/PaginasAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaginas(int id, Paginas paginas)
        {
            if (id != paginas.Id)
            {
                return BadRequest();
            }

            _context.Entry(paginas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaginasExists(id))
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

        // POST: api/PaginasAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Paginas>> PostPaginas([FromForm] Paginas pagina, IFormFile? iconFile)
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
                    pagina.Thumbnail = "defaultIcon.png";   //usa-se uma imagem pre definida
                }
                else   //se for imagem
                {
                    haImagem = true;
                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString();
                    string extensaoImagem = Path.GetExtension(iconFile.FileName).ToLowerInvariant();
                    nomeImagem += extensaoImagem;
                    pagina.Thumbnail = nomeImagem;   //guarda o nome da imagem na BD
                }
            }

            _context.Add(pagina);   //adiciona os dados recebidos ao objeto
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



            return CreatedAtAction("GetPaginas", new { id = pagina.Id }, pagina);
        }

        // DELETE: api/PaginasAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaginas(int id)
        {
            var paginas = await _context.Paginas.FindAsync(id);
            if (paginas == null)
            {
                return NotFound();
            }

            _context.Paginas.Remove(paginas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaginasExists(int id)
        {
            return _context.Paginas.Any(e => e.Id == id);
        }
    }
}
