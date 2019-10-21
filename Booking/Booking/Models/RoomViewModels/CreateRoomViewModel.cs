using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Models.RoomViewModels
{
    public class CreateRoomViewModel
    {
        public int RoomId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string NameRoom { get; set; }


        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        [Display(Name = "From (time)")]
        [DataType(DataType.Time)]
        public TimeSpan MinTime { get; set; }



        [Display(Name = "To (time)")]
        [DataType(DataType.Time)]
        public TimeSpan MaxTime { get; set; }


        [Display(Name = "Max count of people")]
        [Range(0, 20)]
        public int MaxPeople { get; set; }


    }
}