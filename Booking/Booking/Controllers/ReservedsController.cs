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
using PagedList;

namespace Booking.Controllers
{
    [Authorize]
    public class ReservedsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public char[] separateComa = { ',' };

        private readonly string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

        private readonly string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;

        // GET: Reserveds
        public ActionResult Index(int? page)
        {
            var reserveds = db.Reserveds.Include(r => r.Room).ToList();
            //reserveds=reserveds.Where(x => x.UsersId == System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            //reserveds = reserveds.Where(x => x.OwnerId == userId);
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(reserveds.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult SearchRoom(string name)
        {

           
            var reserveds = db.Reserveds.Where(x => x.EventName.Contains(name)).ToList();
           if(name == "")
            {
                reserveds = null;
            }


            return PartialView(reserveds);
        }


        [HttpGet]
        public ActionResult AddList()
        {


            //var reserved = db.Reserveds.Find(ReserveId);
            var allUserNames = db.Users.Select(x => x.UserName);
            //string temp = "";
            //foreach(var item in allUserNames)
            //{
            //    temp += $"{item},";
            //}


            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                //ReservedId = ReserveId,
                UsersEmails = allUserNames.ToArray()
            };



            return View(createReserveViewModel);
        }

        [HttpPost]
        public ActionResult AddList(string[] UsersEmails, CreateReserveViewModel createReserveViewModel)
        {
            //if (ModelState.IsValid)
            //{

            //}


            //createReserveViewModel.ReservedId = ReserveId;
            createReserveViewModel.UsersEmails = UsersEmails;

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
            //ViewBag.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            Reserved reserved = new Reserved
            {
                OwnerId = currentUserId
            };

            return View(reserved);
        }

        // POST: Reserveds/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reserved reserved)
        {
            reserved.OwnerId = currentUserId;
            if (ModelState.IsValid)
            {

                db.Reserveds.Add(reserved);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", reserved.RoomId);

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




            var selected = reserved.SelectedUsersEmails.Split(',');
            
            ViewBag.selected = selected;

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

                reserved.SelectedUsersEmails = string.Join(",", reserved.UsersEmails);
                db.Entry(reserved).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var selected = reserved.SelectedUsersEmails.Split(',');

            ViewBag.selected = selected;

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
