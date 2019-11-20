using Booking.Entity_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking.Models.ReservedViewModels
{
    public class DeleteReserveViewModel
    {
        public DeleteReserveViewModel() { }
        public DeleteReserveViewModel(Reserved reserved)
        {
            ReservedId = reserved.ReservedId;
            EventName = reserved.EventName;
        }
        public int ReservedId { get; set; }
        public string EventName { get; set; }
    }
}