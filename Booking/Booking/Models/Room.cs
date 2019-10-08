using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public TimeSpan MinTime { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan MaxTime { get; set; }
        public int MaxPeople { get; set; }

    }
}