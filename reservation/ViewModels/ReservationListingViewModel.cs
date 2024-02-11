using reservation.Commands;
using reservation.Models;
using reservation.Services;
using reservation.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace reservation.ViewModels
{
    public class ReservationListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ReservationViewModel> _reservations;
        private readonly HotelStore _hotelStore;
        public IEnumerable<ReservationViewModel> Reservations => _reservations;

        public ICommand MakeReservationCommand { get; }
        public ICommand LoadReservationsCommand { get; }
        public ReservationListingViewModel(HotelStore hotelStore, NavigationService makeReservationNavigationService)
        {
            _reservations = new ObservableCollection<ReservationViewModel>();
            _hotelStore = hotelStore;

            LoadReservationsCommand = new LoadReservationsCommand(this, hotelStore);
            MakeReservationCommand = new NavigateCommand(makeReservationNavigationService);

            hotelStore.ReservationMade += OnReservationMade;
        }
        public override void Dispose()
        {
            _hotelStore.ReservationMade -= OnReservationMade;
            base.Dispose();
        }

        private void OnReservationMade(Reservation reservation)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }

        public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, NavigationService makeReservationNavigationService)
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(hotelStore, makeReservationNavigationService);
            viewModel.LoadReservationsCommand.Execute(null);

            return viewModel;
        }
        public void UpdateReservation(IEnumerable<Reservation> reservations)
        {
            _reservations.Clear();
            foreach(Reservation reservation in reservations)
            {
                ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
                _reservations.Add(reservationViewModel);
            }
        }
    }
}
