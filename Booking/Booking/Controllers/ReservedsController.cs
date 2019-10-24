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


        private readonly string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

        private readonly string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;

        private MapperConfiguration CreateReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateReserveViewModel, Reserved>()
            );

        private MapperConfiguration ReservedToEditReserveViewModel = new MapperConfiguration(
            cfg => cfg.CreateMap<Reserved, EditReserveViewModel>()
            );

        private MapperConfiguration EditReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateReserveViewModel, Reserved>()
            );

        public char[] separateComa = { ',' };

        // GET: Reserveds
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            var reserveds = db.Reserveds.Include(r => r.Room).ToList();
            var reserveIndexModel = reserveds
                .OrderBy(a => a.Room.NameRoom)
                .Select(a => new IndexReserveViewModel(a));
            //reserveds = reserveds.Where(x => x.OwnerId == currentUserId).ToList();
            ViewBag.CurrentId = currentUserId;
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


            var reserveds = db.Reserveds.Where(x => x.EventName.Contains(name)).ToList();
            if (name == "")
            {
                reserveds = null;
            }


            return PartialView(reserveds);
        }


        [HttpGet]
        public ActionResult AddList()
        {


            //var reserved = db.Reserveds.Find(ReserveId);
            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            //string temp = "";
            //foreach(var item in allUserNames)
            //{
            //    temp += $"{item},";
            //}

            ViewBag.UsersEmails = new MultiSelectList(allUserNames);

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
            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = new MultiSelectList(allUserNames);
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
            var detailsReserveViewModel = new DetailsReserveViewModel(reserved);
            return View(detailsReserveViewModel);
        }

        // GET: Reserveds/Create
        public ActionResult Create()
        {
            //ViewBag.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                OwnerId = currentUserId,
                
            };
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");

            

            return View(createReserveViewModel);
        }

        // POST: Reserveds/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateReserveViewModel createReserveViewModel)
        {
            createReserveViewModel.OwnerId = currentUserId;
            if (!ModelState.IsValid)
            {
                ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", createReserveViewModel.RoomId);
                return View(createReserveViewModel);
            }

            db.Reserveds.Add(CreateReserveViewModelToReserved.CreateMapper()
                    .Map<CreateReserveViewModel, Reserved>(createReserveViewModel));
            db.SaveChanges();
            return RedirectToAction("Index");
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

            var editReserveViewModel = new EditReserveViewModel(reserved){};

            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = new MultiSelectList(allUserNames);

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

                //reserved.SelectedUsersEmails = string.Join(",", reserved.UsersEmails);
                db.Entry(EditReserveViewModelToReserved.CreateMapper()
                    .Map<EditReserveViewModel, Reserved>(editReserveViewModel)).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //var selected = reserved.SelectedUsersEmails.Split(',');

            //ViewBag.selected = selected;



            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = new MultiSelectList(allUserNames);

            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", editReserveViewModel.RoomId);
            return View(editReserveViewModel);
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
