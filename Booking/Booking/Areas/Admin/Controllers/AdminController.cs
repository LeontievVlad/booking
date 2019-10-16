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
using Microsoft.AspNet.Identity.EntityFramework;

namespace Booking.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [Authorize(Roles = "SuperAdmin")]
        public ActionResult GetRoles()
        {
            var userRoles = new List<RolesViewModel>();
            //var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            //Get all the usernames
            foreach (var user in userStore.Users)
            {
                var r = new RolesViewModel
                {
                    UserName = user.UserName
                };
                userRoles.Add(r);
            }
            //Get all the Roles for our users
            foreach (var user in userRoles)
            {

                user.RoleNames = userManager.GetRoles(userStore.Users.First(s => s.UserName == user.UserName).Id);
                //user.RoleNames = userManager.IsInRole(userStore.Users., "user");
            }

            return View(userRoles);

        }

        [HttpGet]
        public ActionResult ChangeRole(string UserName, string RoleNames)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userStore.Users;

            userManager.RemoveFromRole(userStore.Users.First(s => s.UserName == UserName).Id, RoleNames);
            userManager.AddToRole(userStore.Users.First(s => s.UserName == UserName).Id,
                RoleNames == "User" ? "Admin" : "User");
            return RedirectToAction("GetRoles");
        }




        [Authorize]
        [HttpGet]
        public ActionResult GetRoom(int? id)
        {
            var name = System.Web.HttpContext.Current.User.Identity.Name;
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            ViewBag.userId = userId;
            ViewBag.roomId = id;
            ViewBag.data = $"id: {id}, userId: {userId}, userName: {name}";

            return View(db.Rooms.Find(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetRoom(Reserved reserved)
        {

            //if (ModelState.IsValid)
            //{
            //    //reserved.UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //    db.Reserveds.Add(reserved);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}


            return RedirectToAction("Index");
        }



        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        // GET: Admin/Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Admin/Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Admin/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,NameRoom,Date,MinTime,BusyTime,MaxTime,MaxPeople")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room);
        }

        // GET: Admin/Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Admin/Admin/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,NameRoom,Date,MinTime,BusyTime,MaxTime,MaxPeople")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Admin/Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Admin/Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
