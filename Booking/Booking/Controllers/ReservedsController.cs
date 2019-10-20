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
using Microsoft.AspNet.Identity.EntityFramework;

namespace Booking.Controllers
{
    [Authorize]
    public class ReservedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reserveds

        public ActionResult Index()
        {
            var reserveds = db.Reserveds.Include(r => r.Room);
            //reserveds=reserveds.Where(x => x.UsersId == System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            //reserveds = reserveds.Where(x => x.OwnerId == userId);

            return View(reserveds.ToList());
        }

        [HttpGet]
        public ActionResult AddList()
        {

            var allUserNames = db.Users.Select(x => x.UserName);


            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                UsersEmails = allUserNames.ToArray()
            };



            return View(createReserveViewModel);
        }

        [HttpPost]
        public ActionResult AddList(string[] selectedEmails)
        {

            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                UsersEmails = selectedEmails
            };

            return View(createReserveViewModel);
        }

        // GET: Reserveds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserved reserved = db.Reserveds.Find(id);
            if (reserved == null)
            {
                return HttpNotFound();
            }
            return View(reserved);
        }

        // GET: Reserveds/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");

            return View();
        }

        // POST: Reserveds/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservedId,EventName,ReservedDate,ReservedTimeFrom,ReservedTimeTo,RoomId,UsersId")] Reserved reserved)
        {
            if (ModelState.IsValid)
            {
                reserved.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                db.Reserveds.Add(reserved);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", reserved.RoomId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName", reserved.OwnerId);
            return View(reserved);
        }

        

       

        // GET: Reserveds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserved reserved = db.Reserveds.Find(id);
            if (reserved == null)
            {
                return HttpNotFound();
            }
            var allUserNames = db.Users.Select(x => x.UserName);
            reserved.UsersEmails = allUserNames.ToArray();

            
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", reserved.RoomId);
            return View(reserved);
        }

        // POST: Reserveds/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reserved reserved)
        {
            if (ModelState.IsValid)
            {
                //reserved.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                //var oldreserve = db.Reserveds.Find(reserved.ReservedId);

                //reserved.OwnerId = oldreserve.OwnerId;
                db.Entry(reserved).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", reserved.RoomId);
            return View(reserved);
        }

        // GET: Reserveds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserved reserved = db.Reserveds.Find(id);
            if (reserved == null)
            {
                return HttpNotFound();
            }
            return View(reserved);
        }

        // POST: Reserveds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserved reserved = db.Reserveds.Find(id);
            db.Reserveds.Remove(reserved);
            db.SaveChanges();
            return RedirectToAction("Index");
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
