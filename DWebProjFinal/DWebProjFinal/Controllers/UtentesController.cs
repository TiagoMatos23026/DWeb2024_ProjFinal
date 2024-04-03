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
    public class UtentesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utentes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utentes.ToListAsync());
        }

        // GET: Utentes/Details/5
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telemovel,dataNasc,Biografia")] Utentes utentes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utentes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utentes);
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

        // POST: Utentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telemovel,dataNasc,Biografia")] Utentes utentes)
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtentesExists(int id)
        {
            return _context.Utentes.Any(e => e.Id == id);
        }
    }
}
