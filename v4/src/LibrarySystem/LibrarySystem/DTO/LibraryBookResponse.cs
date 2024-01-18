namespace LibrarySystem.DTO
{
    public class LibraryBookResponse
    {
        public Guid bookUid { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Condition { get; set; }
        public int AvailableCount { get; set; }
    }
}
