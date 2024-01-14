namespace LibrarySystem.DTO
{
    public class PaginationResponse<T>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? TotalElements { get; set; }
        public IEnumerable<T>? Items { get; set; }
    }
}
