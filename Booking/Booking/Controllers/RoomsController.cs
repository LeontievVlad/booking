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

namespace Booking.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        [HttpGet]
        public ActionResult Reserve(int? RoomId)
        {
            ViewBag.work = RoomId;
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult Reserve(Reserved reserved)
        {
            if (ModelState.IsValid)
            {
                db.Reserveds.Add(reserved);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
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
