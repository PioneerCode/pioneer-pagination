namespace Pioneer.Pagination.Example.Models
{
    public class HomeViewModel
    {
        public PaginatedMetaModel Start { get; set; }
        public PaginatedMetaModel Full { get; set; }
        public PaginatedMetaModel End { get; set; }
        public PaginatedMetaModel Subset { get; set; }
        public PaginatedMetaModel Zero { get; set; }
    }
}
