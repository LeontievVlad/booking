using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime MinTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime BusyTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime MaxTime { get; set; }
        public int MaxPeople { get; set; }

    }
}