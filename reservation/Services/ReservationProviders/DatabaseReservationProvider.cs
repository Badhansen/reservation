using Microsoft.EntityFrameworkCore;
using reservation.DBContexts;
using reservation.DTOs;
using reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservation.Services.ReservationProviders
{
    public class DatabaseReservationProvider : IReservationProvider
    {
        private readonly ReservationDbContextFactory _dbContextFactory;
        public DatabaseReservationProvider(ReservationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            using (ReservationDbContext context = _dbContextFactory.CreateDbContext())
            {
                IEnumerable<ReservationDTO> reservationDTOs = await context.Reservations.ToListAsync();
                return reservationDTOs.Select(r => ToReservation(r));
            }
        }
        private static Reservation ToReservation(ReservationDTO r)
        {
            return new Reservation(new RoomID(r.FloorNumber, r.RoomNumber), r.UserName, r.StartTime, r.EndTime);
        }
    }
}
