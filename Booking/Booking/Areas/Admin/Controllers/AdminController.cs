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
        private readonly string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        private readonly string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;
        private readonly MapperConfiguration CreateRoomViewModelToRoom = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateRoomViewModel, Room>()
            );

        private readonly MapperConfiguration EditRoomViewModelToRoom = new MapperConfiguration(
            cfg => cfg.CreateMap<EditRoomViewModel, Room>()
            );


        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult GetRoles()
        {
            ViewBag.CountGuests = CountGuests();
            var userRoles = new List<RolesViewModel>();
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            List<string> disabled = new List<string>();
            if (User.IsInRole("Admin"))
            {
                disabled = db.Roles
                    .Where(x => x.Name == "Admin" && x.Name == "SuperAdmin")
                    .Select(x => x.Name)
                    .ToList();

            }

            //Get all the usernames
            foreach (var user in userStore.Users)
            {
                var r = new RolesViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                    RolesList = db.Roles.Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = x.Name,
                        Disabled = x.Name == "SuperAdmin" ? true : false
                    })
                };
                userRoles.Add(r);
            }
            //Get all the Roles for our users
            foreach (var user in userRoles)
            {
                user.RoleNames = userManager.GetRoles(userStore.Users.First(s => s.UserName == user.UserName).Id);

            }

            //var currentUser = db.Users.Find(currentUserId);
            //ViewBag.CurrentRole = currentUser.Roles.ToString();
            //var ro = db.Roles.Where(x=>x.Id).ToList();
            //List<ApplicationUser> applicationUsers = db.Users.ToList();




            ViewBag.allRoles = new SelectList(db.Roles, "Id", "Name");
            return View(userRoles);

        }



        [HttpGet]
        public ActionResult ChangeRole(RolesViewModel model)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userStore.Users;
            var role = db.Roles.Find(model.ChangeRoleTo);
            userManager.RemoveFromRole(userStore.Users.First(s => s.UserName == model.UserName).Id, model.RoleNames.First());
            userManager.AddToRole(userStore.Users.First(s => s.UserName == model.UserName).Id, role.Name);

            return RedirectToAction("GetRoles");
        }

        public ActionResult AllRoles()
        {
            ViewBag.roles = db.Roles.Select(x => x.Name).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult CreateRole()
        {
            var roles = db.Roles.ToList();

            return View(roles);
        }

        [HttpPost]
        public ActionResult CreateRole(IdentityRole roles)
        {
            if (string.IsNullOrWhiteSpace(roles.Name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.Roles.Add(roles);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
        public int CountGuests()
        {
            //a694828d-76e3-4c7d-9811-0bf136168c8b
            var guest = db.Roles.Where(x => x.Name == "Guest").ToList();
            var users = guest.Select(x => x.Users).ToArray();
            return users[0].Count;
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
            ViewBag.CountGuests = CountGuests();
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
            var reserved = db.Reserveds.Where(x => x.RoomId == id).ToList();
            if (reserved.Count != 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Ця кімната заброньована");
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
