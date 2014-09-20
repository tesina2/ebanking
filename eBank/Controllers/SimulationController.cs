using eBank.DAL;
using eBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace eBank.Controllers
{
    public class SimulationController : Controller
    {
        private BankContext db = new BankContext();

        public ActionResult Index()
        {
            return View();
        }
         
        /** Food Methods Start **/

        [HttpGet]
        public ActionResult Food()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult GotoFancy() 
        {
            Store store = db.Stores.Find(1);
            return PartialView(store);
        }

        [HttpGet]
        public PartialViewResult GotoBurger()
        {
            Store store = db.Stores.Find(2);
            return PartialView(store);
        }

        [HttpGet]
        public PartialViewResult GotoRamen()
        {
            Store store = db.Stores.Find(3);
            return PartialView(store);
        }

        public RedirectToRouteResult PurchaseSingleItem(int id)
        {
            SimulationManager mgr = new SimulationManager();

            var item = db.StoreItems.Find(id);

            var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(userProfile.UserId);

            var accounts = db.Accounts.Where(x => x.StudentID == student.UserId);

            var account = accounts.FirstOrDefault();

            var accountSavings = accounts.Single(x => x.AccountName == "Savings");

            var purchase = mgr.PurchaseSingleItem(item, student, account, accountSavings);

            db.Purchases.Add(purchase);
            db.SaveChanges();

            return RedirectToAction("Index", "Student");
        }

        /** Convience Methods Start **/

        [HttpGet]
        public ActionResult Convenience()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult GotoGas()
        {
            Store store = db.Stores.Find(4);
            return PartialView(store);
        }

        public ActionResult GotoQuickMart()
        {
            return View(db.Stores.Find(6));
        }

        public ActionResult GotoMEGASpeedyStop()
        {
            return View(db.Stores.Find(7));
        }

        [HttpPost]
        public RedirectToRouteResult PurchaseMultipleItems(List<int> MultipleItemsCheckboxes)
        {
            SimulationManager mgr = new SimulationManager();

            ICollection<StoreItem> items = new List<StoreItem>(); //Don't know if this will work

            var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(userProfile.UserId);

            var studentExpenses = student.StudentExpenses; //Not sure if this is right?

            var accounts = db.Accounts.Where(x => x.StudentID == student.UserId);

            var account = accounts.FirstOrDefault();

            var accountSavings = accounts.Single(x => x.AccountName == "Savings");

            if (MultipleItemsCheckboxes != null)
            {
                foreach (var item in MultipleItemsCheckboxes)
                {
                    var storeItem = db.StoreItems.Find(item);
                    items.Add(storeItem);
                }

                //var purchases = mgr.PurchaseMultipleItems(items, student, account, accountSavings, studentExpenses);

                //foreach (var item in purchases)
                //{
                //    db.Purchases.Add(item);
                //}

                db.SaveChanges();

                return RedirectToAction("Index", "Student");
            }
            else return RedirectToAction("Index"); //Set up error 
        }
        
        /** Health Methods Start **/
        public ActionResult Health()
        {
            return View();
        }


        public ActionResult GotoDrugStore()
        {
            return View(db.Stores.Find(8));
        }

        public ActionResult GotoGym()
        {
            return View(db.Stores.Find(9));
        }

        public ActionResult GotoShakeStore()
        {
            return View(db.Stores.Find(10));
        }

        public ActionResult GotoVivaVitamin()
        {
            return View(db.Stores.Find(11));
        }

        public ActionResult GotoHospital()
        {
            return View(db.Stores.Find(12));
        }

        public ActionResult GotoSportingStop()
        {
            return View(db.Stores.Find(13));
        }


        /** Fun Methods Start **/
        public ActionResult Fun()
        {
            return View();
        }

        public ActionResult GotoSkyDiving()
        {
            return View(db.Stores.Find(5));
        }

        public ActionResult GotoMovieTheater()
        {
            return View(db.Stores.Find(14));
        }

        public ActionResult GotoPark()
        {
            return View(db.Stores.Find(15));
        }

        public ActionResult GotoBowling()
        {
            return View(db.Stores.Find(16));
        }

        public ActionResult GotoAmusementPark()
        {
            return View(db.Stores.Find(17));
        }

        public ActionResult GotoTheLake()
        {
            return View(db.Stores.Find(18));
        }

        /** Daily Events **/
        public ActionResult DailyEvents()
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
