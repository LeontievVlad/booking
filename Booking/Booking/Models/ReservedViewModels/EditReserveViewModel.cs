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
        [Required]
        public string EventName { get; set; }

        public string Description { get; set; }

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
        public string AcceptedEmails { get; set; }

        public string DeniedEmails { get; set; }

        public bool IsPrivate { get; set; }
        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        public List<ApplicationUser> GetUserList { get; set; }
    }
}