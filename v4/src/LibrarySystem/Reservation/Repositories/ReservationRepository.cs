using Microsoft.EntityFrameworkCore;
using Reservation.DTO;
using Reservation.Entities;
using Reservation.Interfaces;
using System;

namespace Reservation.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        readonly ReservationDbContext _context;
        public ReservationRepository(ReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Reservations> GetReservationByGuid(Guid reservationGuid)
        {
            return await _context.Reservations.FirstOrDefaultAsync(e => e.Reservation_uid.Equals(reservationGuid));
        }

        public async Task<IEnumerable<ReservationResponse>> GetUserReservations(string userName)
        {
            var reservations = await _context.Reservations.Where(r => r.UserName == userName && r.Status == "RENTED").ToListAsync();
            var reservs = new List<ReservationResponse>();
            foreach (var reservation in reservations)
            {
                reservs.Add(new ReservationResponse() 
                {
                    Book_uid = reservation.Book_uid,
                    Reservation_uid = reservation.Reservation_uid,
                    Library_uid = reservation.Library_uid,
                    Start_date = DateOnly.FromDateTime(reservation.Start_date),
                    Till_date = DateOnly.FromDateTime(reservation.Till_date),
                    Status = reservation.Status,
                    UserName = reservation.UserName
                });
            }
            return reservs;
            
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void CreateReservation(Reservations entity)
        {
            _context.Reservations.Add(entity);
        }
        public void UpdateReservation(Reservations entity)
        {
            _context.Reservations.Update(entity);
        }

        public void DeleteReservation(Reservations entity)
        {
            _context.Reservations.Remove(entity);
        }
    }
}
