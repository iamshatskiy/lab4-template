namespace LibrarySystem.Models
{
    public class Books
    {
        public int? Id { get; set; }
        public Guid Book_uid { get; set; }
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Condition { get; set; }
    }
}
