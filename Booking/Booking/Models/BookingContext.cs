using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Booking.Entity_Models;

namespace Booking.Models
{
    public class BookingContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reserved> Reserveds { get; set; }

        //public DbSet<ApplicationUser> allUsers { get; set; }

    }
}