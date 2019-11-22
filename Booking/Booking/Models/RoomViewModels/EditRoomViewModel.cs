using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "Зображення")]
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public IList<Reserved> Reserveds { get; set; }
    }
}