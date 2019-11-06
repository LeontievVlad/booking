using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Booking.Entity_Models;
using Booking.Models;
using Booking.Models.RoomViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace Booking.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly MapperConfiguration CreateRoomViewModelToRoom = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateRoomViewModel, Room>()
            );

        private readonly MapperConfiguration EditRoomViewModelToRoom = new MapperConfiguration(
            cfg => cfg.CreateMap<EditRoomViewModel, Room>()
            );


        [Authorize(Roles = "SuperAdmin")]
        public ActionResult GetRoles()
        {
            var userRoles = new List<RolesViewModel>();
            var userStore = new UserStore<ApplicationUser>(db);
#pragma warning disable IDE0068 // Использовать рекомендуемый шаблон Dispose
            var userManager = new UserManager<ApplicationUser>(userStore);
#pragma warning restore IDE0068 // Использовать рекомендуемый шаблон Dispose

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

            if (ModelState.IsValid)
            {
                //reserved.UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                db.Reserveds.Add(reserved);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }



        // GET: Admin/Admin
        public ActionResult Index(int? page)
        {

            var rooms = db.Rooms.ToList();
            var indexRoomViewModels = rooms
                .OrderBy(a => a.NameRoom)
                .Select(a => new IndexRoomViewModel(a));
            //reserveds = reserveds.Where(x => x.OwnerId == currentUserId).ToList();
            //ViewBag.CurrentId = currentUserId;
            //reserveds=reserveds.Where(x => x.UsersId == System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            //reserveds = reserveds.Where(x => x.OwnerId == userId);
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(indexRoomViewModels.ToPagedList(pageNumber, pageSize));
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
            var detailsRoomViewModel = new DetailsRoomViewModel(room);
            return View(detailsRoomViewModel);
        }

        // GET: Admin/Admin/Create
        public ActionResult Create()
        {
            CreateRoomViewModel createRoomViewModel = new CreateRoomViewModel();
            return View(createRoomViewModel);
        }

        // POST: Admin/Admin/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRoomViewModel createRoomViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(CreateRoomViewModelToRoom.CreateMapper()
                    .Map<CreateRoomViewModel, Room>(createRoomViewModel));
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(createRoomViewModel);
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

            var editRoomViewModel = new EditRoomViewModel(room) { };
            return View(editRoomViewModel);
        }

        // POST: Admin/Admin/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRoomViewModel editRoomViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(EditRoomViewModelToRoom.CreateMapper()
                    .Map<EditRoomViewModel, Room>(editRoomViewModel)).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(editRoomViewModel);
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
            var deleteRoomViewModel = new DeleteRoomViewModel(room) { };
            return View(deleteRoomViewModel);
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
