using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models.ReservedViewModels
{
    public class EditReserveViewModel
    {
        public EditReserveViewModel(){}
        public EditReserveViewModel(Reserved reserved)
        {
            ReservedId = reserved.ReservedId;
            EventName = reserved.EventName;
            ReservedDate = reserved.ReservedDate;
            ReservedTimeFrom = reserved.ReservedTimeFrom;
            ReservedTimeTo = reserved.ReservedTimeFrom;
            SelectedUsersEmails = reserved.SelectedUsersEmails;
            UsersEmails = reserved.UsersEmails;
            RoomId = reserved.RoomId;
            OwnerId = reserved.OwnerId;
            Room = reserved.Room;
            User = reserved.User;
        }
        public int ReservedId { get; set; }
        [Required]
        public string EventName { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReservedDate { get; set; }

        [DataType(DataType.Time)]
        [DefaultValue("0:0:0")]
        public TimeSpan ReservedTimeFrom { get; set; }

        [DataType(DataType.Time)]
        [DefaultValue("0:0:0")]
        public TimeSpan ReservedTimeTo { get; set; }

        public string[] UsersEmails { get; set; }
        public string SelectedUsersEmails { get; set; }

        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser User { get; set; }
    }
}