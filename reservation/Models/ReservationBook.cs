using reservation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservation.Models
{
    public class ReservationBook
    {
        private readonly List<Reservation> _roomsToReservations;
        public ReservationBook()
        {
            _roomsToReservations = new  List<Reservation>();
        }
        public IEnumerable<Reservation> GetReservations()
        {
            return _roomsToReservations;
        }
        public void AddReservation(Reservation reservation)
        {
            foreach (Reservation existingReservation  in _roomsToReservations)
            {
                if (existingReservation.Conflicts(reservation))
                {
                    throw new ReservationConflictException(existingReservation, reservation);
                }
            }
            _roomsToReservations.Add(reservation);
        }
    }
}
