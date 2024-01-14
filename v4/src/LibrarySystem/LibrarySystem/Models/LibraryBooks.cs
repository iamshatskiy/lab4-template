namespace LibrarySystem.Models
{
    public class LibraryBooks
    {
        public int? Book_id { get; set; }
        public int? Library_id { get; set; }
        public int Available_count { get; set; }
        public virtual Books? Book { get; set; }
        public virtual Libraries? Library { get; set; }
    }
}
