using reservation.DBContexts;
using reservation.DTOs;
using reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservation.Services.ReservationCreators
{
    public class DatabaseReservationCreator : IReservationCreator
    {
        private readonly ReservationDbContextFactory _dbContextFactory;
        public DatabaseReservationCreator(ReservationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task CreateReservation(Reservation rerservation)
        {
            using (ReservationDbContext context = _dbContextFactory.CreateDbContext())
            {
                ReservationDTO reservationDTO = ToReservationDTO(rerservation);
                context.Reservations.Add(reservationDTO);
                await context.SaveChangesAsync();
            }
        }
        private static ReservationDTO ToReservationDTO(Reservation r)
        {
            return new ReservationDTO()
            {
                FloorNumber = r.RoomID?.FloorNumber ?? 0,
                RoomNumber = r.RoomID?.RoomNumber ?? 0,
                UserName = r.UserName,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
            };
        }
    }
}
