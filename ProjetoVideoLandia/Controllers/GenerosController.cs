﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVideoLandia.Data;
using ProjetoVideoLandia.Models;

namespace ProjetoVideoLandia.Controllers
{
    [Authorize(Roles = "2")]
    public class GenerosController : Controller
    {
        private readonly VideoLandiaContext _context;

        public GenerosController(VideoLandiaContext context)
        {
            _context = context;
        }

        // GET: Generoes
        public async Task<IActionResult> Index()
        {
              return _context.Generos != null ? 
                          View(await _context.Generos.ToListAsync()) :
                          Problem("Entity set 'VideoLandiaContext.Generos'  is null.");
        }

        // GET: Generoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Generos == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        // GET: Generoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Generoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Genero genero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genero);
        }

        // GET: Generoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Generos == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }

        // POST: Generoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Genero genero)
        {
            if (id != genero.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneroExists(genero.Id))
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
            return View(genero);
        }

        // GET: Generoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Generos == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        // POST: Generoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Generos == null)
            {
                return Problem("Entity set 'VideoLandiaContext.Generos'  is null.");
            }
            var genero = await _context.Generos.FindAsync(id);
            if (genero != null)
            {
                _context.Generos.Remove(genero);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeneroExists(int id)
        {
          return (_context.Generos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
