namespace LibrarySystem.DTO
{
    public class RentInfoResponse
    {
        public Guid? reservationUid { get; set; }
        public string? status { get; set; }  
        public DateOnly? startDate { get; set; }
        public DateOnly? tillDate { get; set; }
        public BookResponse? book { get; set; }
        public LibraryResponse? library { get; set; }
        public RatingResponse? rating { get; set; }  
    }
}
