using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking.Models.RoomViewModels
{
    public class DeleteRoomViewModel
    {
        public DeleteRoomViewModel() { }
        public DeleteRoomViewModel(Room room)
        {
            RoomId = room.RoomId;
            NameRoom = room.NameRoom;
        }
        public int RoomId { get; set; }
        public string NameRoom { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan MinTime { get; set; }
        public TimeSpan MaxTime { get; set; }
        public int MaxPeople { get; set; }
        public IList<Reserved> Reserveds { get; set; }
    }
}