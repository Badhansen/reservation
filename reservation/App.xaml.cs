using reservation.Exceptions;
using reservation.Models;
using reservation.Stores;
using reservation.ViewModels;
using reservation.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using reservation.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using reservation.Services.ReservationProviders;
using reservation.Services.ReservationCreators;
using reservation.Services.ReservationConflictValidators;

namespace reservation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CONNECTION_STRING = "Data Source=reservation.db";
        private readonly Hotel _hotel;
        private readonly HotelStore _hotelStore;
        private readonly NavigationStore _navigationStore;
        ReservationDbContextFactory _reservationDbContextFactory;

        public App()
        {
            _reservationDbContextFactory = new ReservationDbContextFactory(CONNECTION_STRING);

            IReservationProvider reservationProvider = new DatabaseReservationProvider(_reservationDbContextFactory);
            IReservationCreator reservationCreator = new DatabaseReservationCreator(_reservationDbContextFactory);
            IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_reservationDbContextFactory);

            ReservationBook reservationBook = new ReservationBook(reservationProvider, reservationCreator, reservationConflictValidator);
            _hotel = new Hotel("Badhan Sen's Hotel", reservationBook);
            _hotelStore = new HotelStore(_hotel);
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(CONNECTION_STRING).Options;
            ReservationDbContext dbContext = new ReservationDbContext(options);

            dbContext.Database.Migrate();

            _navigationStore.CurrentViewModel = CreateReservationListingViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(_hotelStore, new NavigationService(_navigationStore, CreateReservationListingViewModel));
        }
        private ReservationListingViewModel CreateReservationListingViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(_hotelStore, new NavigationService(_navigationStore, CreateMakeReservationViewModel));
        }
    }
}
