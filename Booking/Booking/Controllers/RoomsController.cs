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
using Microsoft.AspNet.Identity;

namespace Booking.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
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
        public ActionResult Reserve(Reserved reserved, int RoomId)
        {
            if (ModelState.IsValid)
            {


                reserved.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                   var a = System.Web.HttpContext.Current.User.Identity.Name;
                reserved.UsersEmails = new List<string> { a};
                db.Reserveds.Add(reserved);
                db.SaveChanges();
                return RedirectToAction("Index");

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
