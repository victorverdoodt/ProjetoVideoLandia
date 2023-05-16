using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoVideoLandia.Data;
using ProjetoVideoLandia.Models;
using ProjetoVideoLandia.ViewModels;
using System.Data;

namespace ProjetoVideoLandia.Controllers
{

    public class FilmesController : Controller
    {
        private readonly VideoLandiaContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public FilmesController(VideoLandiaContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Filmes

        public IActionResult Index(int page = 1, int pageSize = 10, [FromQuery] string? busca = null)
        {
            IOrderedQueryable<Filme> filmes = _context.Filmes
                .Include(x => x.AtoresParticipantes)
                    .ThenInclude(x => x.Ator)
                .Include(x => x.Generos)
                    .ThenInclude(x => x.Genero)
                .OrderBy(f => f.DataCriacao)
                    .ThenBy(f => f.Titulo);

            int totalFilmes;

            IQueryable<Filme> filmesPaginados;
            if (!busca.IsNullOrEmpty())
            {
                totalFilmes = filmes.Where(x => x.Ano.ToString() == busca || x.Titulo.ToLower().Contains(busca.ToLower()) || x.Generos.Any(a => a.Genero.Nome.ToLower().Contains(busca.ToLower())) || x.AtoresParticipantes.Any(a => a.Ator.Nome.ToLower().Contains(busca.ToLower()))).Count();
                filmesPaginados = filmes.Where(x => x.Ano.ToString() == busca || x.Titulo.ToLower().Contains(busca.ToLower()) || x.Generos.Any(a => a.Genero.Nome.ToLower().Contains(busca.ToLower())) || x.AtoresParticipantes.Any(a => a.Ator.Nome.ToLower().Contains(busca.ToLower()))).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                totalFilmes = filmes.Count();
                filmesPaginados = filmes.Skip((page - 1) * pageSize).Take(pageSize);
            }
            int totalPages = (int)Math.Ceiling((double)totalFilmes / pageSize);
            var viewModel = new FilmeViewModel
            {
                Filmes = filmesPaginados.ToList(),
                Pagination = new PaginationViewModel
                {
                    TotalItems = totalFilmes,
                    PageSize = pageSize,
                    PageIndex = page,
                    TotalPages = totalPages
                }
            };

            return View(viewModel);
        }

        // GET: Filmes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Filmes == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes
                 .Include(f => f.AtoresParticipantes)
                    .ThenInclude(fp => fp.Ator)
                 .Include(f => f.Generos)
                    .ThenInclude(fp => fp.Genero)
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        [Authorize(Roles = "2")]
        // GET: Filmes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Filmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm][Bind("Id,CodigoDeBarras,Titulo,Ano,Tipo,Preco,DataAdquirida,ValorDeCusto,Situacao,Diretor,FotoDaCapa")] Filme filme, [FromForm]IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Defina o caminho para salvar o arquivo
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/Filmes");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Salve o arquivo na pasta
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Atualize o campo "FotoDaCapa" com o caminho final
                    filme.FotoDaCapa = $"filmes\\{uniqueFileName}";
                }
                _context.Add(filme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filme);
        }


        // GET: Filmes/Edit/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes
                .Include(f => f.AtoresParticipantes)
                    .ThenInclude(fp => fp.Ator)
                 .Include(x => x.Generos)
                    .ThenInclude(a => a.Genero)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (filme == null)
            {
                return NotFound();
            }

            ViewBag.AtorList = new SelectList(await _context.Atores.ToListAsync(), "Id", "Nome");
            ViewBag.GeneroList = new SelectList(await _context.Generos.ToListAsync(), "Id", "Nome");
            return View(filme);
        }

        // POST: Filmes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Filmes/Edit/5
        [HttpPost]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Ano,ValorDeCusto,Diretor")] Filme filme, [FromForm] IFormFile? file)
        {
            if (id != filme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        // Defina o caminho para salvar o arquivo
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/Filmes");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Salve o arquivo na pasta
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Atualize o campo "FotoDaCapa" com o caminho final
                        filme.FotoDaCapa = $"filmes\\{uniqueFileName}";
                    }
                    _context.Update(filme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmeExists(filme.Id))
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

            ViewBag.AtorList = new SelectList(_context.Atores, "Id", "Nome");
            return View(filme);
        }

        [HttpPost("AdicionarAtor")]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAtor([FromForm] int filmeId, [FromForm] int atorId, [FromForm] string personagem)
        {
            var filme = await _context.Filmes.Include(f => f.AtoresParticipantes).FirstOrDefaultAsync(f => f.Id == filmeId);
            var ator = await _context.Atores.FindAsync(atorId);

            if (filme == null || ator == null)
            {
                return NotFound();
            }

            if (!filme.AtoresParticipantes.Any(fp => fp.AtorId == atorId))
            {
                filme.AtoresParticipantes.Add(new FilmeAtor { FilmeId = filmeId, AtorId = atorId, Personagem = personagem });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id = filmeId });
        }

        [HttpPost("RemoveAtor")]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAtor([FromForm] int filmeId, [FromForm] int atorId)
        {
            var filme = await _context.Filmes.Include(f => f.AtoresParticipantes).FirstOrDefaultAsync(f => f.Id == filmeId);
            var ator = await _context.Atores.FindAsync(atorId);

            if (filme == null || ator == null)
            {
                return NotFound();
            }

            var filmeAtor = filme.AtoresParticipantes.FirstOrDefault(fp => fp.AtorId == atorId);

            if (filmeAtor != null)
            {
                filme.AtoresParticipantes.Remove(filmeAtor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id = filmeId });
        }


        // GET: Filmes/Delete/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Filmes == null)
            {
                return NotFound();
            }

            var filme = await _context.Filmes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Filmes == null)
            {
                return Problem("Entity set 'VideoLandiaContext.Filmes'  is null.");
            }
            var filme = await _context.Filmes.FindAsync(id);
            if (filme != null)
            {
                _context.Filmes.Remove(filme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmeExists(int id)
        {
            return (_context.Filmes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
