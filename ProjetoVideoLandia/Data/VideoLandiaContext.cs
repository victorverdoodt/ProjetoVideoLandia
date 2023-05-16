using Microsoft.EntityFrameworkCore;
using ProjetoVideoLandia.Models;

namespace ProjetoVideoLandia.Data
{
    public class VideoLandiaContext : DbContext
    {
        public VideoLandiaContext(DbContextOptions<VideoLandiaContext> options)
            : base(options)
        {
        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Ator> Atores { get; set; }
        public DbSet<FilmeGenero> FilmesGeneros { get; set; }
        public DbSet<FilmeAtor> FilmesAtores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<MensagemContato> MensagensContato { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmeGenero>()
                .HasKey(fg => new { fg.FilmeId, fg.GeneroId });

            modelBuilder.Entity<FilmeAtor>()
                .HasKey(fa => new { fa.FilmeId, fa.AtorId });

            modelBuilder.Entity<MensagemContato>()
              .HasKey(fa => fa.Id);

            modelBuilder.Entity<Usuario>()
               .HasKey(fa => fa.Id);

            modelBuilder.Entity<Filme>(entity =>
            {
                entity.HasMany(f => f.Generos)
                    .WithOne(fg => fg.Filme)
                    .HasForeignKey(fg => fg.FilmeId);
                entity.HasMany(f => f.AtoresParticipantes)
                    .WithOne(fa => fa.Filme)
                    .HasForeignKey(fa => fa.FilmeId);
            });

            modelBuilder.Entity<Ator>()
                .HasMany(a => a.FilmesParticipados)
                .WithOne(fa => fa.Ator)
                .HasForeignKey(fa => fa.AtorId);
        }
    }
}