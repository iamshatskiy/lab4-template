using Reservation.DTO;
using Reservation.Entities;

namespace Reservation.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationResponse>> GetUserReservations(string userName);
        Task<Reservations> GetReservationByGuid(Guid reservationGuid);

        Task<ReservationResponse?> CreateReservation(string userName, Guid bookUid, Guid libraryUid, DateTime tillDate);

        Task<ReservationResponse?> CloseReservation(Guid reservationUid, DateTime closeDate);
        Task<ReservationCountResponse> GetUserReservationCount(string userName);

        Task DeleteReservation(Guid reservationUid);
    }
}
