using eBank.DAL;
using eBank.Models;
using eBank.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBank.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {

        private BankContext db = new BankContext();

        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name) ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            if (db.Teachers.Find(userProfile.UserId) != null)
            {
                return RedirectToAction("Index", "Teacher");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Teacher model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name) ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
                    if (userProfile != null)
                    {
                        var teacher = new Teacher
                        {
                            UserProfile = userProfile,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                        };


                        db.Teachers.Add(teacher);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Teacher");
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Something went wrong, try again.");
                }
            }
            //Something failed, redisplay form
            return RedirectToAction("Create", "Teacher");
        }

        /** Student Operations **/

        public ActionResult ListStudents()
        {
            var teacherProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var teacher = db.Teachers.Find(teacherProfile.UserId);

            var classrooms = db.Classrooms.Where(x => x.TeacherID == teacher.UserId).Select(x => x.ClassroomID);

            var students = db.Students.Where(x => classrooms.Contains(x.ClassroomID)).OrderBy(s => s.Rank);

            return View(students);
        }

        public ActionResult ViewStudent(int id)
        {
            Student student = db.Students.Find(id);
            return View(student);
        }

        [HttpGet]
        public ActionResult ManageStudent(int id)
        {
            Student student = db.Students.Find(id);

            var teacherProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var classrooms = db.Classrooms.Where(x => x.TeacherID == teacherProfile.UserId);

            var selectionList = new SelectList(classrooms, "ClassroomID", "ClassroomName");

            var vm = new ManageStudentViewModel { ListOfClassrooms = selectionList, StudentID = student.UserId };

            return View(vm);
        }

        [HttpPost]
        public ActionResult ManageStudent(ManageStudentViewModel stud)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    Student student = db.Students.Find(stud.StudentID);

                    student.ClassroomID = stud.SelectedClassroomId;

                    db.SaveChanges();
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Something went wrong, try again.");
                }
            }
            return RedirectToAction("ListStudents");
        }

        /** Classroom Operations **/

        public ActionResult ListClassrooms()
        {
            var teacherProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            
            var teacher = db.Teachers.Find(teacherProfile.UserId);
            
            //Need LINQ query that will only show classroooms that belong to the teacher
            var classrooms = db.Classrooms.Where(x => x.TeacherID == teacher.UserId);
            
            return View(classrooms);
        }

        [HttpGet]
        public ActionResult CreateClassroom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateClassroom(Classroom model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                        ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

                    var classroom = new Classroom
                    {
                        ClassroomName = model.ClassroomName,
                        TeacherID = userProfile.UserId

                    };
                    db.Classrooms.Add(classroom);
                    db.SaveChanges();
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Something went wrong, try again.");
                }
            }
            return RedirectToAction("ListClassrooms");
        }    
   
        //Need to include list student and mangage student functionality
        public ActionResult ViewClassroom(int id)
        {
            var classroom = db.Classrooms.Find(id);
            return View(classroom);
        }

        [HttpGet]
        public ActionResult EditClassroom(int id)
        {
            var classroom = db.Classrooms.Find(id);
            return View(classroom);
        }

        [HttpPost]
        public ActionResult EditClassroom(Classroom classroom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(classroom).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListClassrooms");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(classroom);
        }

        [HttpGet]
        public ActionResult DeleteClassroom(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Classroom classroom = db.Classrooms.Find(id);

            if (classroom.Students.Count() > 0)
            {
                ModelState.AddModelError("", "There are still students in this classroom, please move these students before deleting.");
                return RedirectToAction("ListClassrooms");
            }

            return View(classroom);
        }

        [HttpPost]
        public ActionResult DeleteClassroom(int id)
        {
            try
            {
                Classroom classroom = db.Classrooms.Find(id);

                if (classroom.Students.Count() > 0)
                {
                    ModelState.AddModelError("", "There are still students in this classroom, please move these students before deleting.");
                    return RedirectToAction("ListClassrooms");
                }
                
                db.Classrooms.Remove(classroom);
                db.SaveChanges();
            }
            catch(DataException)
            {
                //Log the error (add a variable name after DataException)
                return RedirectToAction("Delete",
                    new System.Web.Routing.RouteValueDictionary { 
                { "id", id }, 
                { "saveChangesError", true } });
            }
            return RedirectToAction("ListClassrooms");
        }

        /** Other Operations **/

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}