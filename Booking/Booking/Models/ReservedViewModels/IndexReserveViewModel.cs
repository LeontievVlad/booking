using Booking.Entity_Models;
using System;

namespace Booking.Models.ReservedViewModels
{
    public class IndexReserveViewModel
    {

        public IndexReserveViewModel(Reserved reserved)
        {
            ReservedId = reserved.ReservedId;
            EventName = reserved.EventName;
            Description = reserved.Description;
            ReservedDate = reserved.ReservedDate;
            ReservedTimeFrom = reserved.ReservedTimeFrom;
            ReservedTimeTo = reserved.ReservedTimeTo;
            SelectedUsersEmails = reserved.SelectedUsersEmails;
            AcceptedEmails = reserved.AcceptedEmails;
            DeniedEmails = reserved.DeniedEmails;
            IsPrivate = reserved.IsPrivate;
            UsersEmails = reserved.UsersEmails;

            RoomId = reserved.RoomId;
            UserId = reserved.UserId;

            Room = reserved.Room;
            User = reserved.User;
        }
        public int ReservedId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime ReservedDate { get; set; }


        public TimeSpan ReservedTimeFrom { get; set; }


        public TimeSpan ReservedTimeTo { get; set; }

        public string[] UsersEmails { get; set; }
        public string SelectedUsersEmails { get; set; }
        public string AcceptedEmails { get; set; }

        public string DeniedEmails { get; set; }

        public bool IsPrivate { get; set; }
        public int? RoomId { get; set; }
        public Room Room { get; set; }

        //public string OwnerId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}