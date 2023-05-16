namespace ProjetoVideoLandia.ViewModels
{
    public class PaginationViewModel
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);

        public int TotalItems { get; set; }
        public int PageSize { get; set; }
    }

}
