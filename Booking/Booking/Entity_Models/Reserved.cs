using Booking.Models;
using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking.Entity_Models
{
    public class Reserved
    {
        public int ReservedId { get; set; }
        public string EventName { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReservedDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime ReservedTimeFrom { get; set; }
        [DataType(DataType.Time)]
        public DateTime ReservedTimeTo { get; set; }

        
        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public string UsersId { get; set; }
        public ApplicationUser User { get; set; }


        //public string UserId = HttpContext.Current.User.Identity.GetUserId();
        //public List<string> UsersEmails{ get; set; }
        //public string UserName = HttpContext.Current.User.Identity.Name;
    }
}