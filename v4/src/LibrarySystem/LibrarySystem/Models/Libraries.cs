namespace LibrarySystem.Models
{
    public class Libraries
    {
        public int? Id { get; set; }
        public Guid Library_uid { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
