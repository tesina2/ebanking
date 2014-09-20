using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBank.DAL;
using eBank.Models;
using System.Data;
using System.Web.Security;
using WebMatrix.WebData;

namespace eBank.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {

        BankContext db = new BankContext();

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }


        /** Student Operations **/

        public ActionResult ListStudents()
        {
            return View(db.Students.ToList().OrderBy(x => x.LastName));
        }

        public ActionResult ViewStudent(int id)
        {
            Student student = db.Students.Find(id);
            return View(student);
        }

        [HttpGet]
        public ActionResult EditStudent(int id)
        {
            Student student = db.Students.Find(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult EditStudent(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {  
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListStudents");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        /*[HttpGet]
        public ActionResult EditAccounts(int id)
        {
            Student stud = db.Students.Find(id);
            var accounts = db.Accounts.Where(x => x.StudentID == stud.UserId);
            return View(accounts);
        }

        [HttpPost]
        public ActionResult EditAccounts(IEnumerable<Account> accounts)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in accounts)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    return RedirectToAction("ListStudents");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(accounts);
        }*/

        [HttpGet]
        public ActionResult DeleteStudent(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Student student = db.Students.Find(id);
            return View(student);
        }

        //TODO: Remove Expenses when deleting
        [HttpPost]
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                var user = db.UserProfiles.SingleOrDefault(u => u.UserId == id);
                var student = db.Students.Find(id);
                var accounts = db.Accounts.Where(x => x.StudentID == student.UserId);
                var bills = student.StudentExpenses;

                if (Roles.GetRolesForUser(user.UserName).Count() > 0)
                {
                    Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                }

                foreach (var account in accounts)
                {
                    db.Accounts.Remove(account);
                }

                //TODO: Add a foreach to remove every bill

                db.Students.Remove(student);

                db.SaveChanges();

                ((SimpleMembershipProvider)System.Web.Security.Membership.Provider).DeleteAccount(user.UserName);
                ((SimpleMembershipProvider)System.Web.Security.Membership.Provider).DeleteUser(user.UserName, true);

                db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                return RedirectToAction("DeleteStudent",
                    new System.Web.Routing.RouteValueDictionary { 
                { "id", id }, 
                { "saveChangesError", true } });
            }
            return RedirectToAction("ListStudents");
        }

        [HttpGet]
        public ActionResult ListTeachers()
        {
            return View(db.Teachers.ToList().OrderBy(t => t.LastName));
        }


        [HttpGet]
        public ActionResult ViewTeacher(int id)
        {
            Teacher teach = db.Teachers.Find(id);
            return View(teach);
        }

        [HttpGet]
        public ActionResult EditTeacher(int id)
        {
            return View(db.Teachers.Find(id));
        }

        [HttpPost]
        public ActionResult EditTeacher(Teacher teach)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(teach).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ViewTeacher", new { id = teach.UserId });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(teach);
        }

        [HttpGet]
        public ActionResult ListClassrooms()
        {
            return View(db.Classrooms.ToList());
        }

        [HttpGet]
        public ActionResult ViewClassroom(int id)
        {
            return View(db.Classrooms.Find(id));
        }

        [HttpGet]
        public ActionResult EditClassroom(int id)
        {
            return View(db.Classrooms.Find(id));
        }

        [HttpPost]
        public ActionResult EditClassroom(Classroom room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(room).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ViewClassroom", new { id = room.ClassroomID });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(room);
        }



        [HttpGet]
        public ActionResult SimulationSettings()
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
