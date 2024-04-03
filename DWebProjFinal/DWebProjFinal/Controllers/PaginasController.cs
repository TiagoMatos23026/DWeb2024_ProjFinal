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
            return View(await _context.Paginas.ToListAsync());
        }

        // GET: Paginas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paginas = await _context.Paginas
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
            return View();
        }

        // POST: Paginas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descricao,Dificuldade,Media,UtenteFK")] Paginas paginas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paginas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paginas);
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
            return View(paginas);
        }

        // POST: Paginas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descricao,Dificuldade,Media,UtenteFK")] Paginas paginas)
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
