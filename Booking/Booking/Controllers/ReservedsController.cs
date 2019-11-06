using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Booking.Entity_Models;
using Booking.Models;
using Booking.Models.ReservedViewModels;
using Booking.Models.RoomViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualBasic.ApplicationServices;
using PagedList;

namespace Booking.Controllers
{
    [Authorize]
    public class ReservedsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();


        private readonly string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

        private readonly string currentUserName = System.Web.HttpContext.Current.User.Identity.Name;

        private readonly MapperConfiguration CreateReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<CreateReserveViewModel, Reserved>()
            );

        private readonly MapperConfiguration ReservedToEditReserveViewModel = new MapperConfiguration(
            cfg => cfg.CreateMap<Reserved, EditReserveViewModel>()
            );

        private readonly MapperConfiguration EditReserveViewModelToReserved = new MapperConfiguration(
            cfg => cfg.CreateMap<EditReserveViewModel, Reserved>()
            );



        public char[] separateComa = { ',' };

        // GET: Rooms
        [AllowAnonymous]
        public ActionResult IndexRoom(int? page)
        {
            var room = db.Rooms.ToList();
            var roomIndexModel = room
                .OrderBy(x => x.NameRoom).Select(x => new IndexRoomViewModel(x));
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.CountInvites = CountInvites();
            return View(roomIndexModel.ToPagedList(pageNumber, pageSize));
        }

        // GET: Reserveds
        public ActionResult Index(int? page)
        {
            var reserveds = db.Reserveds.Include(x => x.User)
                .Include(r => r.Room)
                .Where(x =>
                    x.ReservedDate >= DateTime.Now
                )
                .ToList();

            if (reserveds.Count == 0)
            {
                return View("EmptyPage");
            }

            var reserveIndexModel = reserveds
                .OrderByDescending(x => x.ReservedDate)
                .Select(x => new IndexReserveViewModel(x));
            //reserveds = reserveds.Where(x => x.OwnerId == currentUserId).ToList();
            ViewBag.CurrentId = currentUserId;
            ViewBag.CurrentName = currentUserName;
            ViewBag.CountInvites = CountInvites();
            //reserveds=reserveds.Where(x => x.UsersId == System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            //reserveds = reserveds.Where(x => x.OwnerId == userId);


            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(reserveIndexModel.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult MyEvents(int? page)
        {

            var events = db.Reserveds.Include(x => x.User)
                .Include(x => x.Room)
                .Where(x => (x.UserId == currentUserId ||
                x.SelectedUsersEmails.Contains(currentUserName) ||
                x.AcceptedEmails.Contains(currentUserName)) /*&& x.ReservedDate >= DateTime.Today*/)
                .ToList();

            if (events.Count == 0)
            {
                return View("EmptyPage");
            }

            var reserveIndexModel = events
                .OrderByDescending(a => a.ReservedDate)
                .Select(a => new IndexReserveViewModel(a));

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            ViewBag.CurrentName = currentUserName;
            ViewBag.CountInvites = CountInvites();


            return View(reserveIndexModel.ToPagedList(pageNumber, pageSize));
        }

        public int CountInvites()
        {
            var reserved = db.Reserveds.ToList();

            int count = 0;
            foreach (var item in reserved)
            {
                if (item.SelectedUsersEmails.Contains(currentUserName))
                    count++;
            }

            return count;
        }

        [HttpPost]
        public ActionResult SearchRoom(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                return PartialView();
            }

            var reserveds = db.Reserveds.Include(x => x.User)
                .Include(x => x.Room)
                .Where(x =>
                (
                    x.EventName.Contains(name) ||
                    x.Room.NameRoom.Contains(name) ||
                    x.User.UserName.Contains(name)
                )
                &&
                (
                    x.SelectedUsersEmails.Contains(currentUserName) ||
                    x.AcceptedEmails.Contains(currentUserName) ||
                    x.IsPrivate == false ||
                    x.UserId == currentUserId
                ))
                .ToList();

            if (reserveds.Count == 0)
            {
                return PartialView();
            }

            var reserveIndexModel = reserveds
                .OrderBy(a => a.Room.NameRoom)
                .Select(a => new IndexReserveViewModel(a));

            return PartialView(reserveIndexModel);
        }

        // GET: Reserveds/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserved reserved = db.Reserveds
                .Include(x => x.User)
                .Include(x => x.Room)
                .First(x => x.ReservedId == id);
            //reserved.User = (ApplicationUser)db.Users;

            if (reserved == null)
            {
                return HttpNotFound();
            }
            if (reserved.UserId != currentUserId)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CountInvites = CountInvites();
            var detailsReserveViewModel = new DetailsReserveViewModel(reserved);
            return View(detailsReserveViewModel);
        }

        // GET: Reserveds/Reserve/5
        [HttpGet]
        public ActionResult Reserve(int? RoomId)
        {
            var room = db.Rooms.Find(RoomId);
            ViewBag.CountInvites = CountInvites();



            //createReserveViewModel.GetUserList = db.Users.Select(x =>
            //new ApplicationUser { Id = x.Id, UserName = x.UserName }).ToList();
            //var uEmails = db.Users.Select(x => new ApplicationUser
            //{

            //}).ToList();

            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                ReservedTimeFrom = room.MinTime,
                ReservedTimeTo = room.MaxTime,
                RoomId = RoomId,
                Room = room,
                GetUserList = db.Users.ToList()
            };
            //var allUserNames = db.Users.Select(x => x.UserName).ToList();
            //ViewBag.UsersEmails = allUserNames;
            ViewBag.CurrentUserName = currentUserName;


            ViewBag.NameRoom = room.NameRoom;


            //ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            return View(createReserveViewModel);
        }

        // POST: Reserveds/Reserve/
        [HttpPost]
        public ActionResult Reserve(CreateReserveViewModel createReserveViewModel)
        {

            var room = db.Rooms.Find(createReserveViewModel.RoomId);
            //var reserveds = db.Reserveds.Where(x=>x.RoomId == createReserveViewModel.RoomId);
            //ViewBag.NameRoom = room.NameRoom;
            //ViewBag.UsersEmails = db.Users.Select(x => x.UserName).ToList();

            string message = "";
            if (createReserveViewModel.ReservedDate < DateTime.Today ||
                (createReserveViewModel.ReservedDate == DateTime.Today &&
                createReserveViewModel.ReservedTimeFrom < DateTime.Now.TimeOfDay)
               )
            {
                message += "Уваги ви бронюєте на минулий час <br/>";
            }
            else if (createReserveViewModel.ReservedDate == DateTime.Today &&
                createReserveViewModel.ReservedTimeFrom.TotalMinutes < DateTime.Now.TimeOfDay.TotalMinutes)
            {

            }
            if (createReserveViewModel.ReservedTimeFrom < room.MinTime ||
                createReserveViewModel.ReservedTimeTo > room.MaxTime)
            {
                message += "Час виходить за межі ( "
                    + room.MinTime + " "
                    + room.MaxTime + " ) <br/>";
            }

            if (createReserveViewModel.UsersEmails != null && createReserveViewModel.UsersEmails.Length > room.MaxPeople)
            {
                message += $"Не всі помістяться в кімнаті (максимальна кількість: {room.MaxPeople} ) <br/>";
            }


            return Json(message, JsonRequestBehavior.AllowGet);

        }

        // save
        [HttpPost]
        public async Task<ActionResult> SaveReserve(CreateReserveViewModel createReserveViewModel)
        {
            createReserveViewModel.UserId = currentUserId;
            if (createReserveViewModel.UsersEmails.Length > 0)
            {
                var selecteUsersEmails = db.Users.Where(x =>
                createReserveViewModel.UsersEmails.Contains(x.Id))
                    .Select(x => x.UserName).ToArray();
                createReserveViewModel.SelectedUsersEmails = string.Join(",", selecteUsersEmails);
            }

            Reserved reserved = new Reserved
            {
                ReservedId = createReserveViewModel.ReservedId,
                EventName = createReserveViewModel.EventName,
                Description = createReserveViewModel.Description,
                ReservedDate = createReserveViewModel.ReservedDate,
                ReservedTimeFrom = createReserveViewModel.ReservedTimeFrom,
                ReservedTimeTo = createReserveViewModel.ReservedTimeTo,
                RoomId = createReserveViewModel.RoomId,
                UserId = createReserveViewModel.UserId,
                SelectedUsersEmails = createReserveViewModel.SelectedUsersEmails,
                IsPrivate = createReserveViewModel.IsPrivate,
                UsersEmails = createReserveViewModel.UsersEmails
            };

            db.Reserveds.Add(reserved);

            //db.Reserveds.Add(
            //    CreateReserveViewModelToReserved.CreateMapper()
            //    .Map<CreateReserveViewModel, Reserved>(createReserveViewModel)
            //    );

            db.SaveChanges();


            if (reserved.SelectedUsersEmails != null)
                await Task.Run(async () =>
                {
                    var date = reserved.ReservedDate.ToShortDateString();
                    var id = reserved.ReservedId;
                    var msg = "Вас запрошено на";
                    string emailBody = $"<div class='center-block'>" +
                    $"<p>{msg} {reserved.EventName} <br/>" +
                    $"Дата: {date} <br/>" +
                    $"Час початку: {reserved.ReservedTimeFrom} <br/>" +
                    $"Час кінця: {reserved.ReservedTimeTo} <br/>" +
                    $"Організатор: {currentUserName} <br/></p>";

                    foreach (var email in reserved.SelectedUsersEmails.Split(',').ToArray())
                    {
                        var link = $"https://localhost:44367/Reserveds/AcceptInvite?id={id}&email={email}&accept=";
                        var linkMsg = $"<p><a href = '{link}true' > Прийняти </a> </p> " +
                            $" <p><a href = '{link}false' > Відхилити </a></p></div>";

                        bool isSend = await SendEmailAsync(email, reserved.EventName, emailBody + linkMsg);
                    }

                });

            return Json(reserved.ReservedId, JsonRequestBehavior.AllowGet);
        }


        // GET: Reserveds/Create
        public ActionResult Create()
        {
            //ViewBag.OwnerId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            CreateReserveViewModel createReserveViewModel = new CreateReserveViewModel
            {
                UserId = currentUserId,
                GetUserList = db.Users.ToList()

            };

            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom");
            //ViewBag.UsersEmails = db.Users.Select(x => x.UserName).ToList();
            ViewBag.CountInvites = CountInvites();
            ViewBag.CurrentUserName = currentUserName;

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
            //var reserved = db.Reserveds.Find(id);
            var reserved = db.Reserveds.Include(x => x.Room).Include(x => x.User).First(x => x.ReservedId == id);
            if (reserved == null)
            {
                return HttpNotFound();
            }


            var editReserveViewModel = new EditReserveViewModel(reserved)
            {
                ReservedId = reserved.ReservedId,
                EventName = reserved.EventName,
                Description = reserved.Description,
                ReservedDate = reserved.ReservedDate,
                ReservedTimeFrom = reserved.ReservedTimeFrom,
                ReservedTimeTo = reserved.ReservedTimeTo,
                SelectedUsersEmails = reserved.SelectedUsersEmails,
                AcceptedEmails = reserved.AcceptedEmails,
                DeniedEmails = reserved.DeniedEmails,
                IsPrivate = reserved.IsPrivate,
                UsersEmails = reserved.UsersEmails,

                RoomId = reserved.RoomId,
                UserId = reserved.UserId,

                Room = reserved.Room,
                User = reserved.User,

                GetUserList = db.Users.ToList()
            };


            ViewBag.CountInvites = CountInvites();


            ViewBag.currentUserName = currentUserName;

            List<string> unionEmail = new List<string>();
            if (!string.IsNullOrEmpty(editReserveViewModel.SelectedUsersEmails))
                unionEmail.AddRange(editReserveViewModel.SelectedUsersEmails.Split(',').ToList());
            if (!string.IsNullOrEmpty(editReserveViewModel.AcceptedEmails))
                unionEmail.AddRange(editReserveViewModel.AcceptedEmails.Split(',').ToList());
            if (!string.IsNullOrEmpty(editReserveViewModel.DeniedEmails))
                unionEmail.AddRange(editReserveViewModel.DeniedEmails.Split(',').ToList());

            //ViewBag.selected = unionEmail;
            editReserveViewModel.UsersEmails = unionEmail.ToArray();

            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", editReserveViewModel.RoomId);
            return View(editReserveViewModel);
        }


        [HttpPost]
        public ActionResult Edit(EditReserveViewModel editReserveViewModel)
        {

            var room = db.Rooms.Find(editReserveViewModel.RoomId);

            string message = "";
            if (editReserveViewModel.ReservedDate < DateTime.Today ||
                (editReserveViewModel.ReservedDate == DateTime.Today &&
                editReserveViewModel.ReservedTimeFrom < DateTime.Now.TimeOfDay)
               )
            {
                message += "Уваги ви бронюєте на минулий час <br/>";
            }
            if (editReserveViewModel.ReservedTimeFrom < room.MinTime ||
                editReserveViewModel.ReservedTimeTo > room.MaxTime)
            {
                message += "Час виходить за межі ( "
                    + room.MinTime + " "
                    + room.MaxTime + " ) <br/>";
            }
            if (editReserveViewModel.UsersEmails != null && editReserveViewModel.UsersEmails.Length > room.MaxPeople)
            {
                message += $"Не всі помістяться в кімнаті (максимальна кількість: {room.MaxPeople} ) <br/>";
            }

            return Json(message, JsonRequestBehavior.AllowGet);


        }

        // save edit
        [HttpPost]
        public async Task<ActionResult> SaveEdit(EditReserveViewModel editReserveViewModel)
        {




            if (editReserveViewModel.UsersEmails.Length > 0)
            {
                var selecteUsersEmails = db.Users.Where(x =>
                editReserveViewModel.UsersEmails.Contains(x.Id))
                    .Select(x => x.UserName).ToArray();
                editReserveViewModel.SelectedUsersEmails = string.Join(",", selecteUsersEmails);
            }

            var reserved = db.Reserveds.Find(editReserveViewModel.ReservedId);
            if (reserved == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            reserved.AcceptedEmails = editReserveViewModel.AcceptedEmails;
            reserved.DeniedEmails = editReserveViewModel.DeniedEmails;
            reserved.Description = editReserveViewModel.Description;
            reserved.EventName = editReserveViewModel.EventName;
            reserved.IsPrivate = editReserveViewModel.IsPrivate;
            reserved.ReservedDate = editReserveViewModel.ReservedDate;
            reserved.ReservedTimeFrom = editReserveViewModel.ReservedTimeFrom;
            reserved.ReservedTimeTo = editReserveViewModel.ReservedTimeTo;
            reserved.RoomId = editReserveViewModel.RoomId;
            reserved.SelectedUsersEmails = editReserveViewModel.SelectedUsersEmails;
            reserved.UsersEmails = editReserveViewModel.UsersEmails;

            db.Entry(reserved).State = EntityState.Modified;
            db.SaveChanges();

            if (reserved.SelectedUsersEmails != null)
            {

                var date = reserved.ReservedDate.ToShortDateString();
                var id = reserved.ReservedId;
                var msg = "Деякі зміни в";
                string emailBody = $"<div>" +
                $"<p>{msg} {reserved.EventName} <br/>" +
                $"Дата: {date} <br/>" +
                $"Час початку: {reserved.ReservedTimeFrom} <br/>" +
                $"Час кінця: {reserved.ReservedTimeTo} <br/>" +
                $"Організатор: {currentUserName} <br/></p>";

                foreach (var email in reserved.SelectedUsersEmails.Split(',').ToArray())
                {
                    await Task.Run(async () =>
                    {
                        var link = $"https://localhost:44367/Reserveds/AcceptInvite?id={id}&email={email}&accept=";
                        var linkMsg = $"<p><a href = '{link}true' > Прийняти </a> </p> " +
                                $" <p><a href = '{link}false' > Відхилити </a></p></div>";

                        bool isSend = await SendEmailAsync(email, reserved.EventName, emailBody + linkMsg);
                    });

                }
            }

            return Json(reserved.ReservedId, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> ToCome(int? id, bool toCome)
        {
            //bool toCome = true;
            var reserved = await db.Reserveds.FindAsync(id);
            if (reserved == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (toCome)
            {
                if (!string.IsNullOrEmpty(reserved.AcceptedEmails))
                {

                    reserved.AcceptedEmails = AddEmailToString(currentUserName, reserved.AcceptedEmails);
                }
                else
                {
                    reserved.AcceptedEmails = currentUserName;
                }
                if (!string.IsNullOrEmpty(reserved.DeniedEmails) &&
                    reserved.DeniedEmails.Contains(currentUserName))
                {
                    reserved.DeniedEmails = string.Join(",",
                        RemoveEmailFromString(currentUserName, reserved.DeniedEmails));
                }

                var user = db.Users.Find(reserved.UserId);

                var date = reserved.ReservedDate.ToShortDateString();


                string emailBody = $"<p>User: {currentUserName}</p>" +
                $"<p>Хоче прийти на {reserved.EventName} <br/>" +
                $"Дата: {date} <br/>" +
                $"Час початку: {reserved.ReservedTimeFrom} <br/>" +
                $"Час кінця: {reserved.ReservedTimeTo} <br/>" +
                $"Організатор: {user.UserName} <br/></p>";
                var link = $"https://localhost:44367/Reserveds/AcceptToCome?id={id}&email={currentUserName}&accept=";
                var linkMsg = $"<p><a href = '{link}true' > Прийняти </a> </p> " +
                    $" <p><a href = '{link}false' > Відхилити </a></p></div>";
                await SendEmailAsync(user.UserName, reserved.EventName, emailBody + linkMsg);
            }
            else
            {
                reserved.AcceptedEmails = string.Join(",",
                    RemoveEmailFromString(currentUserName, reserved.AcceptedEmails));
                if (!string.IsNullOrEmpty(reserved.DeniedEmails))
                {
                    reserved.DeniedEmails = AddEmailToString(currentUserName, reserved.DeniedEmails);
                }
                else
                {
                    reserved.DeniedEmails = currentUserName;
                }
            }




            db.Entry(reserved).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
            ViewBag.CountInvites = CountInvites();
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
            return RedirectToAction("MyEvents");
        }

        //---------------------------------------------
        public ActionResult AcceptInvite(int id, string email, bool accept)
        {

            var reserveds = db.Reserveds.Find(id);
            string msg;
            if (reserveds.ReservedDate < DateTime.Today ||
                (reserveds.ReservedDate == DateTime.Today &&
                reserveds.ReservedTimeFrom < DateTime.Now.TimeOfDay))
            {
                msg = "Подія вже відбулася";
            }
            else if ((!string.IsNullOrEmpty(reserveds.AcceptedEmails) && reserveds.AcceptedEmails.Contains(email)) ||
                    (!string.IsNullOrEmpty(reserveds.DeniedEmails) && reserveds.DeniedEmails.Contains(email)))
            {
                msg = "Ви вже дали відповідь";

            }
            else if (!string.IsNullOrEmpty(reserveds.SelectedUsersEmails) &&
                reserveds.SelectedUsersEmails.Contains(email))
            {
                //Прийнято чи відхилено?
                if (accept)
                {
                    if (reserveds.AcceptedEmails != null)
                    {
                        reserveds.AcceptedEmails = AddEmailToString(email, reserveds.AcceptedEmails);
                    }
                    else
                    {
                        reserveds.AcceptedEmails = string.Join(",", email);
                    }
                    msg = "Прийнято";
                }
                else
                {
                    if (reserveds.DeniedEmails != null)
                    {
                        reserveds.DeniedEmails = AddEmailToString(email, reserveds.DeniedEmails);
                    }
                    else
                    {
                        reserveds.DeniedEmails = string.Join(",", email);
                    }
                    msg = "Відхилено";
                }
            }
            else
            {
                msg = "Вас немає в списку запрошених";
            }


            if (!string.IsNullOrEmpty(reserveds.SelectedUsersEmails) && reserveds.SelectedUsersEmails.Contains(email))
            {
                //Є в списку запрошених
                reserveds.SelectedUsersEmails = string.Join(",",
                    RemoveEmailFromString(email, reserveds.SelectedUsersEmails));
            }
            ViewBag.msg = msg;
            ViewBag.CountInvites = CountInvites();
            db.Entry(reserveds).State = EntityState.Modified;
            db.SaveChanges();
            return View();
        }

        public ActionResult AcceptToCome(int id, string email, bool accept)
        {

            var reserveds = db.Reserveds.Find(id);
            string msg;
            if (reserveds.ReservedDate < DateTime.Today ||
                (reserveds.ReservedDate == DateTime.Today &&
                reserveds.ReservedTimeFrom < DateTime.Now.TimeOfDay))
            {
                msg = "Подія вже відбулася";
            }
            else if (accept)
            {

                msg = "Прийнято";
            }
            else
            {

                if (!string.IsNullOrEmpty(reserveds.AcceptedEmails) && reserveds.AcceptedEmails.Contains(email))
                {
                    reserveds.AcceptedEmails = string.Join(",",
                        RemoveEmailFromString(email, reserveds.AcceptedEmails));
                }

                if (reserveds.DeniedEmails != null)
                {
                    reserveds.DeniedEmails = AddEmailToString(email, reserveds.DeniedEmails);
                }
                else
                {
                    reserveds.DeniedEmails = string.Join(",", email);
                }
                msg = "Відхилено";
            }

            ViewBag.msg = msg;
            ViewBag.CountInvites = CountInvites();
            db.Entry(reserveds).State = EntityState.Modified;
            db.SaveChanges();
            return View();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Качество кода", "IDE0067:Ликвидировать объекты перед потерей области", Justification = "<Ожидание>")]
        public async Task<bool> SendEmailAsync(string email, string subject, string emailBody)
        {

            string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
            string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Timeout = 100000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };


            MailMessage mailMessage = new MailMessage(senderEmail, email, subject, emailBody)
            {
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };

            await client.SendMailAsync(mailMessage);

            return true;
        }

        public string[] RemoveEmailFromString(string email, string str)
        {
            string[] UsersEmails = str.Split(',').ToArray();
            string[] selected = new string[UsersEmails.Length - 1];

            int j = 0;
            foreach (var item in UsersEmails)
            {
                if (item != email && !string.IsNullOrWhiteSpace(item))
                {
                    selected[j] = item;
                    j++;
                }
            }
            return selected;
        }


        public string AddEmailToString(string email, string str)
        {
            string[] UsersEmails = str.Split(',').ToArray();
            string[] addEmails = new string[UsersEmails.Length + 1];
            int i = 0;
            foreach (var item in UsersEmails)
            {
                addEmails[i] = item;
                i++;
            }
            addEmails[i] = email;
            return string.Join(",", addEmails);
        }
        //---------------------------------------------


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
