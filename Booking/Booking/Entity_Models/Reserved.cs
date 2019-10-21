﻿using Booking.Models;
using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue("0:0:0")]
        public TimeSpan ReservedTimeFrom { get; set; }

        [DataType(DataType.Time)]
        [DefaultValue("0:0:0")]
        public TimeSpan ReservedTimeTo { get; set; }

        public string[] UsersEmails { get; set; }
        public string SelectedUsersEmails { get; set; }

        public int? RoomId { get; set; }
        public Room Room { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser User { get; set; }


        //public string UserId = HttpContext.Current.User.Identity.GetUserId();
        //public List<string> UsersEmails{ get; set; }
        //public string UserName = HttpContext.Current.User.Identity.Name;
    }
}