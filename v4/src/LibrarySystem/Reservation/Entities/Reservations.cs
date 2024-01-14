namespace Reservation.Entities
{
    public class Reservations
    {
        public int? Id { get; set; }
        public Guid Reservation_uid { get; set; }
        public string UserName { get; set; }
        public Guid Library_uid { get; set; }
        public Guid Book_uid { get; set; }
        public string Status { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime Till_date { get; set; }
    }
}
