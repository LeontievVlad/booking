using System;

namespace Booking.Models
{
    public class SearchViewModel
    {
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime ReservedDate { get; set; }
        public TimeSpan ReservedTimeFrom { get; set; }
        public TimeSpan ReservedTimeTo { get; set; }
        public bool IsPrivate { get; set; }
        public string RoomName { get; set; }
        public string RoomDescription { get; set; }
        public string UserName { get; set; }
        //public string UsersEmails { get; set; }
    }
}