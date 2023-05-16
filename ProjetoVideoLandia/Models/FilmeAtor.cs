using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoVideoLandia.Models
{
    [PrimaryKey(nameof(FilmeId), nameof(AtorId))]
    public class FilmeAtor
    {
        [Key]
        [Column(Order = 1)]
        public int FilmeId { get; set; }
        public Filme Filme { get; set; }
        [Key]
        [Column(Order = 2)]
        public int AtorId { get; set; }
        public Ator Ator { get; set; }
        public string Personagem { get; set; }
    }

}
