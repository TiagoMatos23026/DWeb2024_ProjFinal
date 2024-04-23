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
    public class UtentesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtentesAPIController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> PutUtentes(int id, Utentes utentes)
        {
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

        // POST: api/UtentesAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utentes>> PostUtentes(Utentes utentes)
        {
            _context.Utentes.Add(utentes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtentes", new { id = utentes.Id }, utentes);
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
    }
}
