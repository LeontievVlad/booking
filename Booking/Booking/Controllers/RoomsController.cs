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
using PagedList;

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

        private MapperConfiguration RoomToCreateRoomViewModel = new MapperConfiguration(
            cfg => cfg.CreateMap<Room, CreateRoomViewModel>()
            );
        private MapperConfiguration EditReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateReserveViewModel, Reserved>()
            );


        //CreateRoomViewModel createRoomViewModel = new CreateRoomViewModel();

        //IMapper mapper = RoomToCreateRoomViewModel.CreateMapper();

        //createRoomViewModel = mapper.Map<Room, CreateRoomViewModel>(room1);

        //public IMapper mapper = config.CreateMapper();

        // GET: Rooms
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            //ViewBag.select = db.Rooms.Select(x => x.NameRoom).ToList();

            var room = db.Rooms.ToList();
            var roomIndexModel = room
                .OrderBy(a => a.NameRoom).Select(a => new IndexRoomViewModel(a));
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(roomIndexModel.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddUsersToEvent(int? RoomId)
        {

            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = allUserNames;



            ViewBag.RoomId = RoomId;
            //ViewBag.ReservedId = ReservedId;
            return PartialView();
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
