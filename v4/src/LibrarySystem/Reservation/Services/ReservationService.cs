using Reservation.DTO;
using Reservation.Entities;
using Reservation.Interfaces;
using Reservation.Repositories;

namespace Reservation.Services
{
    public class ReservationService : IReservationService
    {
        readonly IReservationRepository _reservationRepository;
        public ReservationService(IReservationRepository reservationRepository) 
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<ReservationResponse?> CloseReservation(Guid reservationUid, DateTime closeDate)
        {
            var reservation = await _reservationRepository.GetReservationByGuid(reservationUid);
            if (reservation is null || reservation.Status != "RENTED")
            {
                return null;
            }

            reservation.Status = reservation.Till_date >= closeDate ? "RETURNED" : "EXPIRED";

            _reservationRepository.UpdateReservation(reservation);
            await _reservationRepository.SaveAsync();
            var updatedReservation = await _reservationRepository.GetReservationByGuid(reservationUid);
            return new ReservationResponse {
                Book_uid = updatedReservation.Book_uid,
                Reservation_uid = updatedReservation.Reservation_uid,
                Library_uid = updatedReservation.Library_uid,
                Start_date = DateOnly.FromDateTime(updatedReservation.Start_date),
                Till_date = DateOnly.FromDateTime(updatedReservation.Till_date),
                Status = updatedReservation.Status,
                UserName = updatedReservation.UserName
            };
        }

        public async Task DeleteReservation(Guid reservationGuid)
        {
            var reservation = await _reservationRepository.GetReservationByGuid(reservationGuid);
            if (reservation == null)
            { 
                return; 
            }
            _reservationRepository.DeleteReservation(reservation);
            await _reservationRepository.SaveAsync();
        }

        public async Task<ReservationResponse?> CreateReservation(string userName, Guid bookUid, Guid libraryUid, DateTime tillDate)
        {
            var guid = Guid.NewGuid(); 
            var newReservation = new Reservations {
                Book_uid = bookUid,
                Library_uid = libraryUid,
                Start_date = DateTime.Now,
                Till_date = tillDate,
                Status = "RENTED",
                UserName = userName,
                Reservation_uid = guid
            };

            _reservationRepository.CreateReservation(newReservation);
            await _reservationRepository.SaveAsync();

            var createdReservation = await _reservationRepository.GetReservationByGuid(guid);

            return new ReservationResponse
            {
                Book_uid = createdReservation.Book_uid,
                Reservation_uid = createdReservation.Reservation_uid,
                Library_uid = createdReservation.Library_uid,
                Start_date = DateOnly.FromDateTime(createdReservation.Start_date),
                Till_date = DateOnly.FromDateTime(createdReservation.Till_date),
                Status = createdReservation.Status,
                UserName = createdReservation.UserName
            };
        }

        public async Task<Reservations> GetReservationByGuid(Guid reservationGuid)
        {
            return await _reservationRepository.GetReservationByGuid(reservationGuid);
        }

        public async Task<ReservationCountResponse> GetUserReservationCount(string userName)
        {
            var reservs = await _reservationRepository.GetUserReservations(userName);
            return new ReservationCountResponse { reservationCount = reservs.Count() };
        }
        public async Task<IEnumerable<ReservationResponse>> GetUserReservations(string userName)
        {
            return await _reservationRepository.GetUserReservations(userName);
        }
    }
}
