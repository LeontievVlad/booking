﻿using Booking.Models;
using System;

namespace Booking.Entity_Models
{
    public class Reserved
    {
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

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}