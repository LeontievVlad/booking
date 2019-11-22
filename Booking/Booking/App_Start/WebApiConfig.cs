using System.Web.Http;

namespace Booking
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // отключаем возможность вывода данных в формате xml
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
