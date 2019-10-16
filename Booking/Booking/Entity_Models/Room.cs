using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Entity_Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string NameRoom { get; set; }
        public DateTime Date {get;set;}
        public DateTime MinTime { get; set; }
        public DateTime MaxTime { get; set; }
        public int MaxPeople { get; set; }
        public IList<Reserved> Reserveds { get; set; }
    }
}