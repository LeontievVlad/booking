using System.Web.Http;
using Booking.Models.ReservedViewModels;
using Booking.Entity_Models;
using System.Collections.Generic;

namespace Booking.ApiControllers
{
    public class ReservedsController : ApiController
    {
        public string Get()
        {

            string requestInfo = "Контроллер: " + ControllerContext.ControllerDescriptor.ControllerName;
            requestInfo += " Url: " + ControllerContext.Request.RequestUri +
                " " + ControllerContext.Request.Method.Method;
            return requestInfo;
        }
        public void POST(List<Room> rooms)
        {
            
        }
    }
}
            
            
