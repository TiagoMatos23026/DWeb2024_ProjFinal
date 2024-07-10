using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;

namespace DWebProjFinal.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaginasAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaginasAPIController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<ActionResult<Paginas>> PostPaginas(Paginas paginas)
        {
            _context.Paginas.Add(paginas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaginas", new { id = paginas.Id }, paginas);
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
