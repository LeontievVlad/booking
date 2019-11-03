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
            var reserveds = db.Reserveds.Include(x => x.User)
                .Include(r => r.Room)
                .Where(x =>
                (x.ReservedDate >= DateTime.Today /*&& x.IsPrivate == false*/) ||
                ((x.UserId == currentUserId ||
                x.SelectedUsersEmails.Contains(currentUserName) ||
                x.AcceptedEmails.Contains(currentUserName)) &&
                x.ReservedDate >= DateTime.Today))
                .ToList();

            if (reserveds.Count == 0)
            {
                return View("EmptyPage");
            }

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

        public ActionResult MyEvents(int? page)
        {

            var events = db.Reserveds.Include(x => x.User)
                .Include(x => x.Room)
                .Where(x => (x.UserId == currentUserId ||
                x.SelectedUsersEmails.Contains(currentUserName) ||
                x.AcceptedEmails.Contains(currentUserName)) && x.ReservedDate >= DateTime.Today)
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

            return View(reserveIndexModel.ToPagedList(pageNumber, pageSize));
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
            Reserved reserved = db.Reserveds.Find(id);
            if(reserved.UserId != currentUserId)
            {
                return RedirectToAction("Index");
            }
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
            //var reserveds = db.Reserveds.Where(x=>x.RoomId == createReserveViewModel.RoomId);
            //ViewBag.NameRoom = room.NameRoom;
            //ViewBag.UsersEmails = db.Users.Select(x => x.UserName).ToList();

            string message = "";
            if (createReserveViewModel.ReservedDate <= DateTime.Today)
            {
                message += "Забронювати кімнату можна на наступний день або пізніше <br/>";
            }
            if (createReserveViewModel.ReservedTimeFrom < room.MinTime ||
                createReserveViewModel.ReservedTimeTo > room.MaxTime)
            {
                message += "Час виходить за межі ( "
                    + room.MinTime + " "
                    + room.MaxTime + " ) <br/>";
            }


            //var allDate = reserveds.Select(x => x.ReservedDate).ToList();
            //var timeFrom = reserveds.Select(x => x.ReservedTimeFrom).ToList();
            //var timeTo = reserveds.Select(x => x.ReservedTimeTo).ToList();
            //var currentTimeDuration = createReserveViewModel.ReservedTimeTo - createReserveViewModel.ReservedTimeFrom;

            //foreach (var date in allDate)
            //{
            //    if(date == createReserveViewModel.ReservedDate)
            //    {
            //        foreach(var time in timeFrom)
            //        {

            //        message += " <br/>";
            //        }
            //    }
            //}



            return Json(message, JsonRequestBehavior.AllowGet);


        }

        // save
        [HttpPost]
        public async Task<ActionResult> SaveReserve(CreateReserveViewModel createReserveViewModel)
        {
            createReserveViewModel.UserId = currentUserId;
            if (createReserveViewModel.UsersEmails == null)
            {
                //code here
            }
            else if (createReserveViewModel.UsersEmails.Length > 1 &&
                createReserveViewModel.UsersEmails.Contains(currentUserName))
            {
                string[] otherEmails = new string[createReserveViewModel.UsersEmails.Length - 1];
                int j = 0;
                foreach (var item in createReserveViewModel.UsersEmails)
                {
                    if (item != currentUserName)
                    {
                        otherEmails[j] = item;
                        j++;
                    }
                }
                createReserveViewModel.SelectedUsersEmails = string.Join(",", otherEmails);
            }
            else if (createReserveViewModel.UsersEmails.Length > 0)
            {
                createReserveViewModel.SelectedUsersEmails = string.Join(",", createReserveViewModel.UsersEmails);
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

            return Json("Дані збережено", JsonRequestBehavior.AllowGet);
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
                User = reserved.User


            };



            var allUserNames = db.Users.Select(x => x.UserName).ToList();
            ViewBag.UsersEmails = allUserNames;
            ViewBag.selected = currentUserName.ToList();
            if (editReserveViewModel.SelectedUsersEmails != null)
            {
                ViewBag.selected = editReserveViewModel.SelectedUsersEmails.Split(',').ToList();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "NameRoom", editReserveViewModel.RoomId);
            return View(editReserveViewModel);
        }


        [HttpPost]
        public ActionResult Edit(EditReserveViewModel editReserveViewModel)
        {

            var room = db.Rooms.Find(editReserveViewModel.RoomId);

            string message = "";
            if (editReserveViewModel.ReservedDate <= DateTime.Today)
            {
                message += "Забронювати кімнату можна на наступний день або пізніше <br/>";
            }
            if (editReserveViewModel.ReservedTimeFrom < room.MinTime ||
                editReserveViewModel.ReservedTimeTo > room.MaxTime)
            {
                message += "Час виходить за межі ( "
                    + room.MinTime + " "
                    + room.MaxTime + " ) <br/>";
            }

            return Json(message, JsonRequestBehavior.AllowGet);


        }

        // save edit
        [HttpPost]
        public async Task<ActionResult> SaveEdit(EditReserveViewModel editReserveViewModel)
        {
            //editReserveViewModel.UserId = currentUserId;
            if (editReserveViewModel.UsersEmails == null)
            {
                //code here
            }
            else if (editReserveViewModel.UsersEmails.Length > 1 &&
                editReserveViewModel.UsersEmails.Contains(currentUserName))
            {
                string[] otherEmails = new string[editReserveViewModel.UsersEmails.Length - 1];
                int j = 0;
                foreach (var item in editReserveViewModel.UsersEmails)
                {
                    if (item != currentUserName)
                    {
                        otherEmails[j] = item;
                        j++;
                    }
                }
                editReserveViewModel.SelectedUsersEmails = string.Join(",", otherEmails);
            }
            else if (editReserveViewModel.UsersEmails.Length > 0)
            {
                editReserveViewModel.SelectedUsersEmails = string.Join(",", editReserveViewModel.UsersEmails);
            }

            var reserved = db.Reserveds.Find(editReserveViewModel.ReservedId);
            if (reserved == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            reserved.AcceptedEmails = editReserveViewModel.AcceptedEmails;
            reserved.DeniedEmails = editReserveViewModel.DeniedEmails;
            reserved.Description = editReserveViewModel.Description;
            reserved.EventName = editReserveViewModel.Description;
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

            return Json("Дані збережено", JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> ToCome(EditReserveViewModel editReserveViewModel)
        {
            //editReserveViewModel.UserId = currentUserId;
            if (editReserveViewModel.UsersEmails == null)
            {
                //code here
            }
            else if (editReserveViewModel.UsersEmails.Length > 1 &&
                editReserveViewModel.UsersEmails.Contains(currentUserName))
            {
                string[] otherEmails = new string[editReserveViewModel.UsersEmails.Length - 1];
                int j = 0;
                foreach (var item in editReserveViewModel.UsersEmails)
                {
                    if (item != currentUserName)
                    {
                        otherEmails[j] = item;
                        j++;
                    }
                }
                editReserveViewModel.SelectedUsersEmails = string.Join(",", otherEmails);
            }
            else if (editReserveViewModel.UsersEmails.Length > 0)
            {
                editReserveViewModel.SelectedUsersEmails = string.Join(",", editReserveViewModel.UsersEmails);
            }

            var reserved = await db.Reserveds.FindAsync(editReserveViewModel.ReservedId);
            if (reserved == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            reserved.AcceptedEmails = editReserveViewModel.AcceptedEmails;
            reserved.DeniedEmails = editReserveViewModel.DeniedEmails;
            reserved.Description = editReserveViewModel.Description;
            reserved.EventName = editReserveViewModel.Description;
            reserved.IsPrivate = editReserveViewModel.IsPrivate;
            reserved.ReservedDate = editReserveViewModel.ReservedDate;
            reserved.ReservedTimeFrom = editReserveViewModel.ReservedTimeFrom;
            reserved.ReservedTimeTo = editReserveViewModel.ReservedTimeTo;
            reserved.RoomId = editReserveViewModel.RoomId;
            reserved.SelectedUsersEmails = editReserveViewModel.SelectedUsersEmails;
            reserved.UsersEmails = editReserveViewModel.UsersEmails;
            reserved.User = editReserveViewModel.User;

            reserved.AcceptedEmails += string.Join(",", currentUserName);

            db.Entry(reserved).State = EntityState.Modified;
            db.SaveChanges();

            var owner = db.Reserveds.Include(x => x.User)
                .Where(x => x.ReservedId == reserved.ReservedId)
                .Select(x => x.User.UserName).ToString();


            var date = reserved.ReservedDate.ToShortDateString();
            var id = reserved.ReservedId;
            var msg = "Прийде на";
            string emailBody = $"<p>{currentUserName}</p>" +
            $"<p>{msg} {reserved.EventName} <br/>" +
            $"Дата: {date} <br/>" +
            $"Час початку: {reserved.ReservedTimeFrom} <br/>" +
            $"Час кінця: {reserved.ReservedTimeTo} <br/>" +
            $"Організатор: {owner} <br/></p>";

            bool isSend = await SendEmailAsync(owner, reserved.EventName, emailBody);

            return Json("Дані збережено", JsonRequestBehavior.AllowGet);
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
            return RedirectToAction("MyEvents");
        }

        //---------------------------------------------
        public ActionResult AcceptInvite(int id, string email, bool accept)
        {

            var reserveds = db.Reserveds.Find(id);

            if (reserveds.ReservedDate <= DateTime.Today)
            {
                ViewBag.msg = "Подія вже відбулася";
                reserveds.SelectedUsersEmails = null;
            }
            else if ((reserveds.AcceptedEmails != null && reserveds.AcceptedEmails.Contains(email)) ||
                    (reserveds.DeniedEmails != null && reserveds.DeniedEmails.Contains(email)))
            {
                ViewBag.msg = "Ви вже дали відповідь";
                if (reserveds.SelectedUsersEmails.Contains(email))
                {
                    reserveds.UsersEmails = reserveds.SelectedUsersEmails.Split(',').ToArray();

                    string[] allEmails = new string[reserveds.UsersEmails.Length - 1];

                    int j = 0;
                    foreach (var item in reserveds.UsersEmails)
                    {
                        if (item != email)
                        {
                            allEmails[j] = item;
                            j++;
                        }
                    }

                    reserveds.SelectedUsersEmails = string.Join(",", allEmails);
                }
            }
            else if (reserveds.SelectedUsersEmails != null && reserveds.SelectedUsersEmails.Contains(email))
            {
                //Є в списку запрошених

                //int id = Convert.ToInt32(reserveds.ReservedId);
                //Reserved reserved = db.Reserveds.Find(id);

                //var deleteEmail = 
                reserveds.UsersEmails = reserveds.SelectedUsersEmails.Split(',').ToArray();

                string[] otherEmails = new string[reserveds.UsersEmails.Length - 1];

                int j = 0;
                foreach (var item in reserveds.UsersEmails)
                {
                    if (item != email)
                    {
                        otherEmails[j] = item;
                        j++;
                    }
                }

                reserveds.SelectedUsersEmails = string.Join(",", otherEmails);



                //Прийнято чи відхилено?
                if (accept)
                {
                    if (reserveds.AcceptedEmails != null)
                    {
                        reserveds.UsersEmails = reserveds.AcceptedEmails.Split(',').ToArray();
                        string[] arrEmails = new string[reserveds.UsersEmails.Length + 1];
                        //arrEmails = reserveds.UsersEmails;
                        //arrEmails[reserveds.UsersEmails.Length] = email;

                        j = 0;
                        foreach (var item in reserveds.UsersEmails)
                        {
                            arrEmails[j] = item;
                            j++;
                        }
                        arrEmails[j] = email;
                        reserveds.AcceptedEmails = string.Join(",", arrEmails);
                    }
                    else
                    {
                        reserveds.AcceptedEmails = string.Join(",", email);
                    }
                    ViewBag.msg = "Прийнято";
                }
                else
                {
                    if (reserveds.DeniedEmails != null)
                    {
                        reserveds.UsersEmails = reserveds.DeniedEmails.Split(',').ToArray();
                        otherEmails = reserveds.UsersEmails;
                        otherEmails[otherEmails.Length] = email;
                        reserveds.DeniedEmails = string.Join(",", otherEmails);
                    }
                    else
                    {
                        reserveds.DeniedEmails = string.Join(",", email);
                    }
                    ViewBag.msg = "Відхилено";
                }

                db.Entry(reserveds).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ViewBag.msg = "Вас немає в списку запрошених";
            }

            
            

            return View();
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string emailBody)
        {

            string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
            string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Timeout = 100000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);


            MailMessage mailMessage = new MailMessage(senderEmail, email, subject, emailBody);
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            await client.SendMailAsync(mailMessage);

            return true;
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
