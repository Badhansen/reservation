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
        public MakeReservationViewModel MakeReservationViewModel { get; }
        public IEnumerable<ReservationViewModel> Reservations => _reservations;

        public ICommand MakeReservationCommand { get; }
        public ICommand LoadReservationsCommand { get; }
        public ReservationListingViewModel(HotelStore hotelStore, MakeReservationViewModel makeReservationViewModel, NavigationService makeReservationNavigationService)
        {
            _reservations = new ObservableCollection<ReservationViewModel>();

            MakeReservationViewModel = makeReservationViewModel;

            LoadReservationsCommand = new LoadReservationsCommand(this, hotelStore);
            MakeReservationCommand = new NavigateCommand(makeReservationNavigationService);

            hotelStore.ReservationMade += OnReservationMade;
        }

        private void OnReservationMade(Reservation reservation)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }

        public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore,
           MakeReservationViewModel makeReservationViewModel, NavigationService makeReservationNavigationService)
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(hotelStore, makeReservationViewModel, makeReservationNavigationService);
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
