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


        private string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

        private string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;

        private MapperConfiguration CreateReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateReserveViewModel, Reserved>()
            );

        private MapperConfiguration ReservedToEditReserveViewModel = new MapperConfiguration(
            cfg => cfg.CreateMap<Reserved, EditReserveViewModel>()
            );

        private MapperConfiguration EditReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<EditReserveViewModel, Reserved>()
            );

        public char[] separateComa = { ',' };

        // GET: Reserveds
        
        public ActionResult Index(int? page)
        {
            var reserveds = db.Reserveds.Include(x => x.User).Include(r => r.Room).ToList();

            var reserveIndexModel = reserveds
                .OrderBy(a => a.Room.NameRoom)
                .Select(a => new IndexReserveViewModel(a));
            //reserveds = reserveds.Where(x => x.OwnerId == currentUserId).ToList();
            ViewBag.CurrentId = currentUserId;
            ViewBag.CurrentName = currentUserName;
            //reserveds=reserveds.Where(x => x.UsersId == System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            //reserveds = reserveds.Where(x => x.OwnerId == userId);
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(reserveIndexModel.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult SearchRoom(string name)
        {
            
           

            var reserveds = db.Reserveds.Include(x => x.User)
                .Include(x => x.Room)
                .Where(x => x.EventName
                .Contains(name))
                .ToList();


            return PartialView(reserveds);
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
            var detailsReserveViewModel = new DetailsReserveViewModel(reserved);
            return View(detailsReserveViewModel);
        }

        // GET: Reserveds/Reserve/5
        [HttpGet]
        public ActionResult Reserve(int? RoomId)
        {
            var room = db.Rooms.Find(RoomId);

            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                ReservedTimeFrom = room.MinTime,
                ReservedTimeTo = room.MaxTime,
                RoomId = RoomId
            };
            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = allUserNames;
            ViewBag.selected = currentUserName;
            if (createReserveViewModel.SelectedUsersEmails != null)
            {
                ViewBag.selected = createReserveViewModel.SelectedUsersEmails.Split(',').ToList();
            }
            ViewBag.NameRoom = room.NameRoom;
            //ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
            return View(createReserveViewModel);
        }

        // POST: Reserveds/Reserve/
        [HttpPost]
        public ActionResult Reserve(CreateReserveViewModel createReserveViewModel)
        {

            var room = db.Rooms.Find(createReserveViewModel.RoomId);
            ViewBag.NameRoom = room.NameRoom;
            ViewBag.UsersEmails = db.Users.Select(x => x.UserName).ToList();

            if (createReserveViewModel.ReservedTimeFrom < room.MinTime ||
                createReserveViewModel.ReservedTimeTo > room.MaxTime)
            {
                return Json("час виходить за межі ( "
                    + room.MinTime + " "
                    + room.MaxTime + " )",
                    JsonRequestBehavior.AllowGet);
            }

            return Json("success", JsonRequestBehavior.AllowGet);


        }


        [HttpPost]
        public JsonResult SaveReserve(CreateReserveViewModel createReserveViewModel)
        {
            createReserveViewModel.UserId = currentUserId;
            if (createReserveViewModel.UsersEmails != null)
                createReserveViewModel.SelectedUsersEmails = string
                    .Join(",", createReserveViewModel.UsersEmails);

            db.Reserveds.Add(
                CreateReserveViewModelToReserved.CreateMapper()
                .Map<CreateReserveViewModel, Reserved>(createReserveViewModel)
                );

            db.SaveChanges();

            return Json("Saved", JsonRequestBehavior.AllowGet);
        }



        // GET: Reserveds/Create
        public ActionResult Create()
        {
            //ViewBag.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                UserId = currentUserId,

            };

            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            ViewBag.UsersEmails = db.Users.Select(x => x.UserName).ToList();



            return View(createReserveViewModel);
        }

        // POST: Reserveds/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateReserveViewModel createReserveViewModel)
        {
            createReserveViewModel.UserId = currentUserId;
            if (!ModelState.IsValid)
            {

                ViewBag.UsersEmails = db.Users.Select(x => x.UserName).ToList();
                ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", createReserveViewModel.RoomId);
                return Json("model is not valid", JsonRequestBehavior.AllowGet);
            }
            if (createReserveViewModel.UsersEmails != null)
                createReserveViewModel.SelectedUsersEmails = string.Join(",", createReserveViewModel.UsersEmails);
            db.Reserveds.Add(CreateReserveViewModelToReserved.CreateMapper()
                    .Map<CreateReserveViewModel, Reserved>(createReserveViewModel));
            db.SaveChanges();
            return Json("success", JsonRequestBehavior.AllowGet);
        }


        // GET: Reserveds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reserved = db.Reserveds.Find(id);


            if (reserved == null)
            {
                return HttpNotFound();
            }

            //var allUserNames = db.Users.Select(x => x.UserName);
            //reserved.UsersEmails = allUserNames.ToArray();




            //var selected = reserved.SelectedUsersEmails.Split(',');

            //ViewBag.selected = selected;

            var editReserveViewModel = new EditReserveViewModel(reserved) { };



            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = allUserNames;
            ViewBag.selected = currentUserName;
            if (editReserveViewModel.SelectedUsersEmails != null)
            {
                ViewBag.selected = editReserveViewModel.SelectedUsersEmails.Split(',').ToList();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", editReserveViewModel.RoomId);
            return View(editReserveViewModel);
        }

        // POST: Reserveds/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditReserveViewModel editReserveViewModel)
        {
            if (ModelState.IsValid)
            {
                //reserved.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                //var oldreserve = db.Reserveds.Find(reserved.ReservedId);
                //reserved.OwnerId = oldreserve.OwnerId;

                //Reserved reserved = Mapper.Map<EditReserveViewModel, Reserved>(editReserveViewModel);

                //IMapper mapper = RoomToCreateRoomViewModel.CreateMapper();
                //CreateRoomViewModel createRoomViewModel = new CreateRoomViewModel();
                //createRoomViewModel = mapper.Map<Room, CreateRoomViewModel>(room1);
                if (editReserveViewModel.UsersEmails != null)
                    editReserveViewModel.SelectedUsersEmails = string.Join(",", editReserveViewModel.UsersEmails);
                db.Entry((Reserved)EditReserveViewModelToReserved.CreateMapper()
                    .Map<EditReserveViewModel, Reserved>(editReserveViewModel)).State = EntityState.Modified;
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            //var selected = reserved.SelectedUsersEmails.Split(',');

            //ViewBag.selected = selected;



            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = allUserNames;
            ViewBag.selected = currentUserName;
            if (editReserveViewModel.SelectedUsersEmails != null)
            {
                ViewBag.selected = editReserveViewModel.SelectedUsersEmails.Split(',').ToList();
            }
            //ViewBag.selected = currentUserName.ToList();
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", editReserveViewModel.RoomId);
            return Json("model is not valid", JsonRequestBehavior.AllowGet);
        }

        // GET: Reserveds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reserved = db.Reserveds.Find(id);
            if (reserved == null)
            {
                return HttpNotFound();
            }

            var deleteReserveViewModel = new DeleteReserveViewModel(reserved);
            return View(deleteReserveViewModel);
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
