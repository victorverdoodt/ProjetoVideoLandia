using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoVideoLandia.Data;
using ProjetoVideoLandia.Models;
using ProjetoVideoLandia.ViewModels;
using System.Data;

namespace ProjetoVideoLandia.Controllers
{
    public class AtoresController : Controller
    {
        private readonly VideoLandiaContext _context;

        public AtoresController(VideoLandiaContext context)
        {
            _context = context;
        }

        // GET: Ators
        [AllowAnonymous]
        public IActionResult Index(int page = 1, int pageSize = 10, [FromQuery] string? busca = null)
        {
            IOrderedQueryable<Ator> filmes = _context.Atores
                .Include(x => x.FilmesParticipados)
                .OrderBy(f => f.Nome);

            int totalFilmes;

            IQueryable<Ator> filmesPaginados;
            if (!busca.IsNullOrEmpty())
            {
                totalFilmes = filmes.Where(x => x.Nome.ToLower().Contains(busca.ToLower())).Count();
                filmesPaginados = filmes.Where(x => x.Nome.ToLower().Contains(busca.ToLower())).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                totalFilmes = filmes.Count();
                filmesPaginados = filmes.Skip((page - 1) * pageSize).Take(pageSize);
            }
            int totalPages = (int)Math.Ceiling((double)totalFilmes / pageSize);
            var viewModel = new AtorViewModel
            {
                Atores = filmesPaginados.ToList(),
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

        // GET: Ators

        /*
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        using (var client = new ServiceClient("139d294335441494f3e8cd3319eff122"))
        {
            for (int i = 1, count = 10; i <= count; i++)
            {
                var movies = await client.Movies.GetTopRatedAsync(null, i, cancellationToken);
                count = movies.PageCount; // keep track of the actual page count

                foreach (Movie m in movies.Results)
                {
                    var movie = await client.Movies.GetAsync(m.Id, null, true, cancellationToken);
                    foreach(var q in movie.Genres)
                    {
                        if(!await _context.Generos.AnyAsync(x=> x.Nome == q.Name))
                        {
                            await _context.Generos.AddAsync(new Genero
                            {
                                Nome = q.Name
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    var director = movie.Credits.Crew.FirstOrDefault(c => c.Job == "Director");
                    var newMovie = new Filme
                    {
                        Ano = movie.ReleaseDate.GetValueOrDefault().Year,
                        Titulo = movie.Title,
                        Sinopse = movie.Overview,
                        FotoDaCapa = $"Filmes{movie.Poster}",
                        Diretor = director.Name,
                        ValorDeCusto = movie.Budget
                    };
                    await _context.Filmes.AddAsync(newMovie);
                    if (movie.Poster != null)
                    {
                        string filepath = Path.Combine("Filmes", movie.Poster.TrimStart('/'));
                        await DownloadImage(movie.Poster, filepath, cancellationToken);
                    }
                    await _context.SaveChangesAsync();
                    foreach (var q in movie.Genres)
                    {
                        var gen = await _context.Generos.FirstOrDefaultAsync(x => x.Nome == q.Name);
                        if (gen != null)
                        {
                            await _context.FilmesGeneros.AddAsync(new FilmeGenero { FilmeId = newMovie.Id, GeneroId = gen.Id});
                        }
                    }
                    await _context.SaveChangesAsync();
                    var personIds = movie.Credits.Cast.Select(s => new { Id = s.Id, Personagem = s.Character});

                    foreach (var id in personIds.Take(10))
                    {
                        var person = await client.People.GetAsync(id.Id, true, cancellationToken);
                        var OldActor = await _context.Atores.FirstOrDefaultAsync(x => x.Nome == person.Name);
                        if (OldActor == null)
                        {
                            var newAtor = new Ator { Nome = person.Name, DataDeNascimento = person.BirthDay != null ? DateTime.Parse(person.BirthDay) : null, Foto = $"Atores{person.Poster}", PaisDeNascimento = person.BirthPlace ?? "não informado" };
                            await _context.Atores.AddAsync(newAtor);
                            await _context.SaveChangesAsync();
                            await _context.FilmesAtores.AddAsync(new FilmeAtor { AtorId = newAtor.Id, FilmeId = newMovie.Id, Personagem = id.Personagem });
                        }
                        else
                        {
                            await _context.FilmesAtores.AddAsync(new FilmeAtor { AtorId = OldActor.Id, FilmeId = newMovie.Id, Personagem = id.Personagem });
                        }
                        await _context.SaveChangesAsync();
                        if (person.Poster != null)
                        {
                            string filepath2 = Path.Combine("Atores", person.Poster.TrimStart('/'));
                            await DownloadImage(person.Poster, filepath2, cancellationToken);
                        }
                    }
                }
            }
        }



        static async Task DownloadImage(string filename, string localpath, CancellationToken cancellationToken)
        {
            if (!System.IO.File.Exists(localpath))
            {
                string folder = Path.GetDirectoryName(localpath);
                Directory.CreateDirectory(folder);

                var storage = new StorageClient();
                using (var fileStream = new FileStream(localpath, FileMode.Create,
                    FileAccess.Write, FileShare.None, short.MaxValue, true))
                {
                    try { await storage.DownloadAsync(filename, fileStream, cancellationToken); }
                    catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }
                }
            }
        }*/

        // GET: Ators/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Atores == null)
            {
                return NotFound();
            }

            var ator = await _context.Atores
                .Include(x => x.FilmesParticipados).ThenInclude(x => x.Filme).FirstOrDefaultAsync(m => m.Id == id);
            if (ator == null)
            {
                return NotFound();
            }

            return View(ator);
        }

        // GET: Ators/Create
        [Authorize(Roles = "2")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataDeNascimento,PaisDeNascimento,Foto")] Ator ator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ator);
        }

        // GET: Ators/Edit/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Atores == null)
            {
                return NotFound();
            }

            var ator = await _context.Atores.FindAsync(id);
            if (ator == null)
            {
                return NotFound();
            }
            return View(ator);
        }

        // POST: Ators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataDeNascimento,PaisDeNascimento,Foto")] Ator ator)
        {
            if (id != ator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtorExists(ator.Id))
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
            return View(ator);
        }

        // GET: Ators/Delete/5
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Atores == null)
            {
                return NotFound();
            }

            var ator = await _context.Atores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ator == null)
            {
                return NotFound();
            }

            return View(ator);
        }

        // POST: Ators/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Atores == null)
            {
                return Problem("Entity set 'ProjetoVideoLandiaContext.Ator'  is null.");
            }
            var ator = await _context.Atores.FindAsync(id);
            if (ator != null)
            {
                _context.Atores.Remove(ator);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtorExists(int id)
        {
            return (_context.Atores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
