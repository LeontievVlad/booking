using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Booking.Models.RoomViewModels
{
    public class IndexRoomViewModel
    {
        public IndexRoomViewModel(Room room)
        {
            RoomId = room.RoomId;
            NameRoom = room.NameRoom;
            Description = room.Description;
            MinTime = room.MinTime;
            MaxTime = room.MaxTime;
            MaxPeople = room.MaxPeople;
            Reserveds = room.Reserveds;
            Image = room.Image;
            ImageFile = room.ImageFile;
        }
        public int RoomId { get; set; }
        public string NameRoom { get; set; }
        [Display(Name = "Опис кімнати")]
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan MinTime { get; set; }
        public TimeSpan MaxTime { get; set; }
        public int MaxPeople { get; set; }
        [Display(Name = "Зображення")]
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public IList<Reserved> Reserveds { get; set; }
    }
}