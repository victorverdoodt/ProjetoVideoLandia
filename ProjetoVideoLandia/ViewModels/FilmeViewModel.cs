using ProjetoVideoLandia.Models;

namespace ProjetoVideoLandia.ViewModels
{
    public class FilmeViewModel
    {
        public PaginationViewModel Pagination { get; set; }
        public List<Filme> Filmes { get; set; }
    }

}
