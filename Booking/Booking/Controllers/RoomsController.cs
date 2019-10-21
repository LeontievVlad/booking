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
using Booking.Models.RoomViewModels;
using Booking.Models.ReservedViewModels;
using Microsoft.AspNet.Identity;

namespace Booking.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private readonly string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

        private readonly string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;

        // GET: Rooms
        [AllowAnonymous]
        public ActionResult Index()
        {



            //ViewBag.select = db.Rooms.Select(x => x.NameRoom).ToList();
            return View(db.Rooms.ToList());
        }

        [HttpGet]
        public ActionResult Reserve()
        {

            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {

            };



            //ViewBag.work = RoomId;
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
            return View(createReserveViewModel);
        }

        [HttpPost]
        public ActionResult Reserve(CreateReserveViewModel createReserveViewModel)
        {
            if (ModelState.IsValid)
            {


                //createReserveViewModel.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                string[] currentName = { currentUserName };
                //createReserveViewModel.UsersEmails = new List<string> { a};

                Reserved reserve = new Reserved
                {
                    ReservedId = createReserveViewModel.ReservedId,
                    ReservedDate = createReserveViewModel.ReservedDate,
                    ReservedTimeFrom = createReserveViewModel.ReservedTimeFrom,
                    ReservedTimeTo = createReserveViewModel.ReservedTimeTo,
                    EventName = createReserveViewModel.EventName,
                    RoomId = createReserveViewModel.RoomId,
                    OwnerId = currentUserId,
                    UsersEmails = currentName

                };

                db.Reserveds.Add(reserve);
                db.SaveChanges();
                return RedirectToAction("Index", "Reserveds");

            }

            //ViewBag.work = RoomId;
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
