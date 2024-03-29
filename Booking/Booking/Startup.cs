﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Booking.Startup))]
namespace Booking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
