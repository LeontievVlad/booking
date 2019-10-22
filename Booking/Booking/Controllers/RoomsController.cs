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
using AutoMapper;

namespace Booking.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private readonly string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

        private readonly string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;

        private MapperConfiguration CreateReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateReserveViewModel, Reserved>()
            );

        //public IMapper mapper = config.CreateMapper();

        // GET: Rooms
        [AllowAnonymous]
        public ActionResult Index()
        {



            //ViewBag.select = db.Rooms.Select(x => x.NameRoom).ToList();
            return View(db.Rooms.ToList());
        }

        [HttpGet]
        public ActionResult Reserve(Room room)
        {

            var createReserve = new CreateReserveViewModel
            {
                OwnerId = currentUserId,
                RoomId = room.RoomId
            };


            ViewBag.RoomName = room.NameRoom;
            //ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult Reserve(CreateReserveViewModel createReserveViewModel)
        {
            if (ModelState.IsValid)
            {

                createReserveViewModel.OwnerId = currentUserId;

                db.Reserveds.Add(
                    CreateReserveViewModelToReserved.CreateMapper()
                    .Map<CreateReserveViewModel, Reserved>(createReserveViewModel
                    ));

                db.SaveChanges();

                return RedirectToAction("Index", "Reserveds");

            }



            ViewBag.RoomName = createReserveViewModel.Room.NameRoom;
            //ViewBag.work = RoomId;
            //ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", createReserveViewModel.RoomId);
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName",);
            return View(createReserveViewModel);
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
