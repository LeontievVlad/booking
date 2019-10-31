using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models.RoomViewModels
{
    public class EditRoomViewModel
    {
        public EditRoomViewModel() { }
        public EditRoomViewModel(Room room)
        {
            RoomId = room.RoomId;
            NameRoom = room.NameRoom;
            MinTime = room.MinTime;
            MaxTime = room.MaxTime;
            MaxPeople = room.MaxPeople;
            Reserveds = room.Reserveds;
        }
        public int RoomId { get; set; }
        public string NameRoom { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        [Display(Name = "From (time)")]
        [DataType(DataType.Time)]
        [DefaultValue("0:0:0")]
        public TimeSpan MinTime { get; set; }



        [Display(Name = "To (time)")]
        [DataType(DataType.Time)]
        [DefaultValue("0:0:0")]
        public TimeSpan MaxTime { get; set; }
        public int MaxPeople { get; set; }
        public IList<Reserved> Reserveds { get; set; }
    }
}