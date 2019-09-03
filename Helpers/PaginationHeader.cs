namespace NomoBucket.API.Helpers
{
    public class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public PaginationHeader(int currentPage, int itemsPerPage, int totalCount, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalCount = totalCount;
            this.TotalPages = totalPages;
        }
    }
  
}