using Microsoft.EntityFrameworkCore;
using reservation.DTOs;
using reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservation.DBContexts
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions options) : base(options) 
        {
            
        }
        public DbSet<ReservationDTO> Reservations { get; set; }

    }
}
