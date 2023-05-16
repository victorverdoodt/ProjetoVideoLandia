using System.ComponentModel.DataAnnotations;

namespace ProjetoVideoLandia.Models
{
    public class Filme
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }
        public decimal ValorDeCusto { get; set; }
        public string Diretor { get; set; }
        public string Sinopse { get; set; }
        public string FotoDaCapa { get; set; }
        public DateTime? DataCriacao { get; set; }
        public ICollection<FilmeGenero> Generos { get; set; }
        public ICollection<FilmeAtor> AtoresParticipantes { get; set; }
    }

}
