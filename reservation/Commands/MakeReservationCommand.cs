using reservation.Exceptions;
using reservation.Models;
using reservation.Services;
using reservation.Stores;
using reservation.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace reservation.Commands
{
    public class MakeReservationCommand : AsyncCommandBase
    {
        private readonly MakeReservationViewModel _makeReservationViewModel;
        private readonly HotelStore _hotelStore;
        private readonly NavigationService _reservationViewNavigationService;

        public MakeReservationCommand(MakeReservationViewModel makeReservationViewModel, HotelStore hotelStore,
            NavigationService reservationViewNavigationService)
        {
            _makeReservationViewModel = makeReservationViewModel;
            _hotelStore = hotelStore;
            _reservationViewNavigationService = reservationViewNavigationService;
            _makeReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MakeReservationViewModel.UserName))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_makeReservationViewModel.UserName) && 
                base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            Reservation reservation = new Reservation(
                new RoomID(
                    _makeReservationViewModel.FloorNumber, _makeReservationViewModel.RoomNumber),
                    _makeReservationViewModel.UserName,
                    _makeReservationViewModel.StartDate, 
                    _makeReservationViewModel.EndDate);
            try
            {
                await _hotelStore.MakeReservation(reservation);
                MessageBox.Show("Successfully reservation completed", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _reservationViewNavigationService.Navigate();
            }
            catch (ReservationConflictException) 
            {
                MessageBox.Show("This room is alread taken", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Fail to make reservation", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
