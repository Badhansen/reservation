using reservation.Exceptions;
using reservation.Services.ReservationConflictValidators;
using reservation.Services.ReservationCreators;
using reservation.Services.ReservationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservation.Models
{
    public class ReservationBook
    {
        private readonly IReservationProvider _reservationProvider;
        private readonly IReservationCreator _reservationCreator;
        private readonly IReservationConflictValidator _reservationConflictValidator;

        public ReservationBook(IReservationProvider reservationProvider, IReservationCreator reservationCreator, IReservationConflictValidator reservationConflictValidator)
        {
            _reservationProvider = reservationProvider;
            _reservationCreator = reservationCreator;
            _reservationConflictValidator = reservationConflictValidator;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _reservationProvider.GetAllReservations();
        }
        public async Task AddReservation(Reservation reservation)
        {
            Reservation conflictingReservaion = await _reservationConflictValidator.GetConflictingReservatoin(reservation);
            if (conflictingReservaion != null)
            {
                throw new ReservationConflictException(conflictingReservaion, reservation);
            }
            await _reservationCreator.CreateReservation(reservation);
        }
    }
}
