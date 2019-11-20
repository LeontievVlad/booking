using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Models
{
    public class RolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<string> RoleNames { get; set; }
        public string[] ChangeRoleTo { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}