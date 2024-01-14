namespace LibrarySystem.DTO
{
    public class RentingRequest
    {
        public string bookUid { get; set; }
        public string libraryUid { get; set; }
        public DateOnly tillDate { get; set; }
    }
}
