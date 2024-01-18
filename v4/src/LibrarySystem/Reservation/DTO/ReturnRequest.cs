namespace Reservation.DTO
{
    public class ReturnRequest
    {
        public string reservationGuid { get; set; }
        public DateOnly returnDate { get; set; }
    }
}
