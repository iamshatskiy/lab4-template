using Reservation.DTO;
using Reservation.Entities;

namespace Reservation.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<ReservationResponse>> GetUserReservations(string userName);
        Task<Reservations> GetReservationByGuid(Guid reservationGuid);

        void CreateReservation(Reservations entity);
        void UpdateReservation(Reservations entity);
        void DeleteReservation(Reservations entity);
        Task SaveAsync();
    }
}
