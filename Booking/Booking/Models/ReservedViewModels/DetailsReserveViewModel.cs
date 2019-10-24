using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking.Models.ReservedViewModels
{
    public class DetailsReserveViewModel
    {
        public DetailsReserveViewModel() { }
        public DetailsReserveViewModel(Reserved reserved)
        {
            ReservedId = reserved.ReservedId;
            EventName = reserved.EventName;
            ReservedDate = reserved.ReservedDate;
            ReservedTimeFrom = reserved.ReservedTimeFrom;
            ReservedTimeTo = reserved.ReservedTimeFrom;
            SelectedUsersEmails = reserved.SelectedUsersEmails;
            RoomId = reserved.RoomId;
            OwnerId = reserved.OwnerId;
            Room = reserved.Room;
            User = reserved.User;

        }
        public int ReservedId { get; set; }
        public string EventName { get; set; }

        public DateTime ReservedDate { get; set; }


        public TimeSpan ReservedTimeFrom { get; set; }


        public TimeSpan ReservedTimeTo { get; set; }

        public string[] UsersEmails { get; set; }
        public string SelectedUsersEmails { get; set; }

        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser User { get; set; }
    }
}