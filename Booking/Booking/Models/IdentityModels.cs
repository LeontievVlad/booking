using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Booking.Entity_Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Booking.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reserved> Reserveds { get; set; }
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<Booking.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}