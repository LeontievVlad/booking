using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models.ReservedViewModels
{
    public class CreateReserveViewModel
    {
        public int ReservedId { get; set; }
        public string EventName { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReservedDate { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan ReservedTimeFrom { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan ReservedTimeTo { get; set; }

        public List<string> UsersEmails { get; set; }

        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser User { get; set; }


        
    }
}