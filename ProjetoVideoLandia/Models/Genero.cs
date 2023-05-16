namespace ProjetoVideoLandia.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<FilmeGenero> FilmesDoGenero { get; set; }
    }
}
