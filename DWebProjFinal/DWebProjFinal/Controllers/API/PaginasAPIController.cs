using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace DWebProjFinal.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaginasAPIController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PaginasAPIController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: api/PaginasAPI
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paginas>>> GetPaginas()
        {
            return await _context.Paginas.ToListAsync();
        }

        // GET: api/PaginasAPI/5
        [AllowAnonymous]
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

        // GET: api/PaginasAPI/utente/UtenteFK
        [AllowAnonymous]
        [HttpGet("utente/{utenteFK}")]
        public async Task<ActionResult<IEnumerable<Paginas>>> GetPaginasByUtente(int utenteFK)
        {
            var paginas = await _context.Paginas.Where(p => p.UtenteFK == utenteFK).ToListAsync();

            if (paginas == null)
            {
                return NotFound();
            }

            return paginas;
        }


        // PUT: api/PaginasAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaginas([FromForm] Paginas pagina, [FromForm] IFormFile? ImgThumbnail)
        {
            var userAtual = _userManager.GetUserId(User);

            var utente = _context.Utentes.FirstOrDefault(m => m.Id == pagina.UtenteFK);

            if (userAtual != utente.UserID)
            {
                return BadRequest();
            }

            //-----------------------------//
            //Algoritmo para upload de imagem
            //-----------------------------//
            string nomeImagem = "";
            bool haImagem = false;

            // há ficheiro?
            if (ImgThumbnail == null)
            {
                pagina.Thumbnail = "defaultThumbnail.png";
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

            _context.Entry(pagina).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/PaginasAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Paginas>> PostPaginas([FromForm] Paginas pagina, [FromForm] IFormFile ImgThumbnail)
        {
            var userAtual = _userManager.GetUserId(User);

            var utente = _context.Utentes.FirstOrDefault(m => m.Id == pagina.UtenteFK);

            if (userAtual != utente.UserID)
            {
                return BadRequest();
            }

            //-----------------------------//
            //Algoritmo para upload de imagem
            //-----------------------------//
            string nomeImagem = "";
            bool haImagem = false;

            // há ficheiro?
            if (ImgThumbnail == null)
            {
                pagina.Thumbnail = "defaultThumbnail.png";
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

            _context.Paginas.Add(pagina);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/PaginasAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaginas(int id)
        {
            var userAtual = _userManager.GetUserId(User);
            var pagina = await _context.Paginas.FindAsync(id);
            var utente = _context.Utentes.FirstOrDefault(m => m.Id == pagina.UtenteFK);

            if (utente.UserID != userAtual)
            {
                return BadRequest();
            }
            if (pagina == null)
            {
                return NotFound();
            }

            _context.Paginas.Remove(pagina);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaginasExists(int id)
        {
            return _context.Paginas.Any(e => e.Id == id);
        }
    }
}
