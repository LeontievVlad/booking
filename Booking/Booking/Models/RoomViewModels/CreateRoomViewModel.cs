using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Booking.Models.RoomViewModels
{
    public class CreateRoomViewModel
    {
        public int RoomId { get; set; }

        [Required]
        [Display(Name = "Name")]
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


        [Display(Name = "Max count of people")]
        [Range(0, 20)]
        public int MaxPeople { get; set; }

        [Display(Name = "Зображення")]
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

    }
}