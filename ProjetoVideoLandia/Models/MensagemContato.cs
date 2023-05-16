using System.ComponentModel.DataAnnotations;

namespace ProjetoVideoLandia.Models
{
    public class MensagemContato
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Assunto { get; set; }
        [Required]
        public string Mensagem { get; set; }
    }
}
