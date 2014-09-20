using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBank.DAL;
using eBank.Models;
using eBank.ViewModels;

namespace eBank.Controllers
{
    public class HomeController : Controller
    {
        private BankContext db = new BankContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to eBank: The online banking simulator! Please login in to access your account or feel free to explore the site.";

            return View();
        }

        public ActionResult About()
        {
            var data = new StudentInformationViewModel()
            {
                StudentCount = db.Students.Count(),
                ClassroomCount = db.Classrooms.Count()
            };
            return View(data);                                                             
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult CreationError()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
