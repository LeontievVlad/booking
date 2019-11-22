using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Booking.Entity_Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string NameRoom { get; set; }
        public string Description { get; set; }
        public TimeSpan MinTime { get; set; }
        public TimeSpan MaxTime { get; set; }
        public int MaxPeople { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public IList<Reserved> Reserveds { get; set; }
    }
}