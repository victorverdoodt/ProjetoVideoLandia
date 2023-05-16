using System.ComponentModel.DataAnnotations;

namespace ProjetoVideoLandia.Models
{
    public class Ator
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataDeNascimento { get; set; }
        public string? PaisDeNascimento { get; set; }
        public string? Foto { get; set; }
        public ICollection<FilmeAtor>? FilmesParticipados { get; set; }
    }

}
