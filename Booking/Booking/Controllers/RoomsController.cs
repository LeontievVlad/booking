using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking.Entity_Models;
using Booking.Models;
using Booking.Models.ReservedViewModels;
using Microsoft.AspNet.Identity;

namespace Booking.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        [AllowAnonymous]
        public ActionResult Index()
        {
            

            //ViewBag.select = db.Rooms.Select(x => x.NameRoom).ToList();
            return View(db.Rooms.ToList());
        }

        [HttpGet]
        public ActionResult Reserve(int? RoomId)
        {

            



            ViewBag.work = RoomId;
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult Reserve(CreateReserveViewModel createReserveViewModel, int RoomId)
        {
            if (ModelState.IsValid)
            {


                //createReserveViewModel.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                   string[] currentName = { System.Web.HttpContext.Current.User.Identity.Name };
                //createReserveViewModel.UsersEmails = new List<string> { a};

                Reserved reserve = new Reserved
                {
                    ReservedId = createReserveViewModel.ReservedId,
                    ReservedDate = createReserveViewModel.ReservedDate,
                    ReservedTimeFrom = createReserveViewModel.ReservedTimeFrom,
                    ReservedTimeTo = createReserveViewModel.ReservedTimeTo,
                    EventName = createReserveViewModel.EventName,
                    RoomId = createReserveViewModel.RoomId,
                    OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    UsersEmails = currentName

                };

                db.Reserveds.Add(reserve);
                db.SaveChanges();
                return RedirectToAction("Index", "Reserveds");

            }

            ViewBag.work = RoomId;
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
