using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoVideoLandia.Models
{
    [PrimaryKey(nameof(FilmeId), nameof(GeneroId))]
    public class FilmeGenero
    {
        [Key]
        [Column(Order = 1)]
        public int FilmeId { get; set; }
        public Filme Filme { get; set; }
        [Key]
        [Column(Order = 2)]
        public int GeneroId { get; set; }
        public Genero Genero { get; set; }
    }
}
