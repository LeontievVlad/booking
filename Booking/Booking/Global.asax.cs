using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using Booking.Models;
using System.Data.Entity;

namespace Booking
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
        }
    }
}
