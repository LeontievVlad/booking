using System.Web.Http;
using Booking.Models.ReservedViewModels;

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
    }
}
            
            
