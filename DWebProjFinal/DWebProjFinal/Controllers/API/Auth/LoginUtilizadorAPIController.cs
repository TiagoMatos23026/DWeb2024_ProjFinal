using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWebProjFinal.Data;
using DWebProjFinal.Models;

namespace DWebProjFinal.Controllers.API.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginUtilizadorAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginUtilizadorAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LoginUtilizadorAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginUtilizador>>> GetLoginUtilizador()
        {
            return await _context.LoginUtilizador.ToListAsync();
        }

        // GET: api/LoginUtilizadorAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoginUtilizador>> GetLoginUtilizador(int id)
        {
            var loginUtilizador = await _context.LoginUtilizador.FindAsync(id);

            if (loginUtilizador == null)
            {
                return NotFound();
            }

            return loginUtilizador;
        }

        // PUT: api/LoginUtilizadorAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginUtilizador(int id, LoginUtilizador loginUtilizador)
        {
            if (id != loginUtilizador.Id)
            {
                return BadRequest();
            }

            _context.Entry(loginUtilizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginUtilizadorExists(id))
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

        // POST: api/LoginUtilizadorAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoginUtilizador>> PostLoginUtilizador(LoginUtilizador loginUtilizador)
        {
            _context.LoginUtilizador.Add(loginUtilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoginUtilizador", new { id = loginUtilizador.Id }, loginUtilizador);
        }

        // DELETE: api/LoginUtilizadorAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginUtilizador(int id)
        {
            var loginUtilizador = await _context.LoginUtilizador.FindAsync(id);
            if (loginUtilizador == null)
            {
                return NotFound();
            }

            _context.LoginUtilizador.Remove(loginUtilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoginUtilizadorExists(int id)
        {
            return _context.LoginUtilizador.Any(e => e.Id == id);
        }
    }
}
