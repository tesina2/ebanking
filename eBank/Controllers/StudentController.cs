using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBank.ViewModels;
using eBank.Models;
using eBank.DAL;
using System.Web.Security;
using System.Data;
using WebMatrix.WebData;

namespace eBank.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private BankContext db = new BankContext();

        //
        // GET: /Student/

        public ActionResult Index()
        {
            var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(userProfile.UserId);

            return View(student);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name) ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            if (db.Students.Find(userProfile.UserId) != null)
            {
                return RedirectToAction("Index", "Student");
            }

            return View();
        }

        //TODO: Add Security
        [Authorize]
        [HttpPost]
        public ActionResult Create(Student model) 
        {
            var userProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            Account CheckingsAccount = new Account { AccountName = "Checkings", AccountTotal = 100, StudentID = userProfile.UserId };
            Account SavingsAccount = new Account { AccountName = "Savings", AccountTotal = 100, StudentID = userProfile.UserId };

            //Additional Expenses will be included after purchase of things such as a house or other items.
           
            StudentExpense PhoneBill = new StudentExpense { ExpenseAccountNumber = "123456789", Name = "Phone Payment", Company = "Telecom Co.", bMissedPayment = false, CurrentAmountOwed = 0, StudentID = userProfile.UserId, bIsRecurring = false, ExpenseID = 1 };

            Classroom classroom = db.Classrooms.Find(model.ClassroomID);

            Student student = new Student
            {
                UserId = userProfile.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                ClassroomID = model.ClassroomID,
                bBillsPaid = true,
                bAlertSetup = false,
                AccountThreshold = 0,
                AlertAccountID = null,
                Happiness = 50,
                Health = 70,
                Hunger = 60,
                Social = 50,
                Energy = 80,
                Points = 0,
                CreditScore = 0
            };

            if (ModelState.IsValid && (classroom != null))
            {
                try
                {        
                    if (userProfile != null)
                    {                    
                        db.Students.Add(student);
                        db.SaveChanges();

                        db.Entry(student).State = EntityState.Modified;
                        db.SaveChanges();

                        db.Accounts.Add(CheckingsAccount);
                        db.SaveChanges();

                        db.Accounts.Add(SavingsAccount);
                        db.SaveChanges();

                        db.StudentExpenses.Add(PhoneBill);
                        db.SaveChanges();

                        return RedirectToAction("Index", "Student");
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("","Something went wrong, try again.");
                }
            }
            return RedirectToAction("Create");
        }

        /** Account Operations **/

        public ActionResult AccountSummary()
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(studentProfile.UserId);

            var accounts = db.Accounts.Where(x => x.StudentID == student.UserId);

            return View(accounts);
        }

        [HttpGet]
        public ActionResult TransferFunds()
        {
            //Get logged in user
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            //Get student account associated with logged in user
            var student = db.Students.Find(studentProfile.UserId);
            //Get accounts associated with student account
            var accounts = db.Accounts.Where(x => x.StudentID == student.UserId);

            var selectionList = new SelectList(accounts, "AccountID", "AccountName");

            var vm = new TransferFundsViewModel { ListOfSourceAccounts = selectionList, ListOfDestinationAccounts = selectionList };

            return View(vm);
        }

        [HttpPost]
        public ActionResult TransferFunds(TransferFundsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccountManager manager = new AccountManager();
                    Account srcAccount = db.Accounts.Find(viewModel.SelectedSourceAccountId);
                    Account destAccount = db.Accounts.Find(viewModel.SelectedDestinationAccountId);

                    manager.TransferMoney(srcAccount, destAccount, viewModel.TransferAmount);
                    db.SaveChanges();
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Something went wrong, transfer could not be completed.");
                }
            }
            return RedirectToAction("AccountSummary");
        }

        [HttpGet]
        public ActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewProfile()
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(studentProfile.UserId);

            return View(student);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(studentProfile.UserId);

            return View(student);
        }

        [HttpPost]
        public ActionResult EditProfile(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Could not save changes, pleae try again.");
                }
            }
            return RedirectToAction("ViewProfile");
        }


        /** Expenses/Bill-Pay Methods **/

        //TODO: Need to create some messaging, notification system that will handle what happens if a student does not add a bill that needs to be payed.

        [HttpGet]
        public ActionResult CreateExpense()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateExpense(AddExpenseViewModel viewModel)
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name) ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            Expense expense = db.Expenses.SingleOrDefault(e => e.ExpenseAccountNumber == viewModel.ExpenseAccountNumber);

            if (expense != null && ModelState.IsValid)
            {
                if (expense.bIsActive == true)
                {
                    StudentExpense studentExpense = new StudentExpense
                    {
                        bIsRecurring = false, //Currently false by default, student user must set up recurring payment on different page.
                        bMissedPayment = false,
                        CurrentAmountOwed = 0,
                        ExpenseAccountNumber = viewModel.ExpenseAccountNumber,
                        Name = viewModel.Name,
                        ExpenseID = expense.ExpenseID,
                        Company = expense.Company,
                        StudentID = studentProfile.UserId
                    };

                    db.StudentExpenses.Add(studentExpense);
                    db.SaveChanges();

                    return RedirectToAction("ViewExpenses");
                }
            }
            return View();  //TODO: Add redirect or error message for if there is an error or if account number does not exist.
        }

        //TODO: Organize View So It Looks Nice
        [HttpGet]
        public ActionResult ViewExpenses()
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name) ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var student = db.Students.Find(studentProfile.UserId);

            return View(student.StudentExpenses);
        }

        //TODO: Add in functionality so it will add in this a an expense/bill under the eSTATEMENT
        [HttpGet]
        public ActionResult PayExpense(int expenseId)
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name) ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var student = db.Students.Find(studentProfile.UserId);
            var accounts = db.Accounts.Where(x => x.StudentID == student.UserId);
            var selectedExpense = db.StudentExpenses.Find(expenseId);
            var selectionList = new SelectList(accounts, "AccountID", "AccountName");

            var vm = new PayExpenseViewModel { ListOfAccounts = selectionList, ExpenseAmount = selectedExpense.CurrentAmountOwed, StudentExpenseId = selectedExpense.StudentExpenseID };
            
            return View(vm);
        }

        [HttpPost]
        public ActionResult PayExpense(PayExpenseViewModel viewModel)
        {

            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name) ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var student = db.Students.Find(studentProfile.UserId);
            var studentExpense = db.StudentExpenses.Find(viewModel.StudentExpenseId); 
            var srcAccount = db.Accounts.Find(viewModel.SelectedAccountId);

            AccountManager manager = new AccountManager();

            if (ModelState.IsValid && srcAccount != null && studentExpense != null)
            {
                try
                {
                    manager.PayExpense(srcAccount, studentExpense, student, false); //bIsLate is currently defaulted to false need to implement system to check if the payment is late
                    db.SaveChanges();

                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Entry(studentExpense).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Something went wrong, the transaction could not be completed.");
                }
            }
            return RedirectToAction("ViewExpenses");
        }       

        [HttpGet]
        public ActionResult SetupAlert()
        {
            //Get logged in user
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            //Get student account associated with logged in user
            var student = db.Students.Find(studentProfile.UserId);
            //Get accounts associated with student account
            var accounts = db.Accounts.Where(x => x.StudentID == student.UserId);

            var selectionList = new SelectList(accounts, "AccountID", "AccountName");

            var vm = new SetupAlertViewModel { ListOfAccounts = selectionList };

            return View(vm);
        }

        [HttpPost]
        public ActionResult SetupAlert(SetupAlertViewModel viewModel)
        {
            Account account = db.Accounts.Find(viewModel.SelectedAccountId);

            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(studentProfile.UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    student.AlertAccountID = viewModel.SelectedAccountId;
                    student.bAlertSetup = true;
                    student.AccountThreshold = viewModel.AmountThreshold;
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Something went wrong, transfer could not be completed.");
                }
            }
            return RedirectToAction("AccountSummary");
        }

        //TODO: Create a ViewModel full of collections of Purchases, Deposits, and Transfers. There three will be used on the statement generation.

        [HttpGet]
        public ActionResult eSTATEMENT()
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var purchases = db.Purchases.Where(x => x.StudentID == studentProfile.UserId);

            //var store = purchases.First().StoreItem.Store; Attempting eager loading

            return View(purchases);
        }

        [HttpGet]
        public ActionResult ViewItem(int id)
        {
            var purchase = db.Purchases.Find(id);
            return View(purchase);
        }

        [HttpGet]
        public PartialViewResult SimulationStores()
        {
            return PartialView();
        }

        [HttpGet] //Points work, but the ranking is being weird
        public PartialViewResult RankAndPoints()
        {
            var studentProfile = db.UserProfiles.Local.SingleOrDefault(u => u.UserName == User.Identity.Name)
                ?? db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);

            var student = db.Students.Find(studentProfile.UserId);

            IEnumerable<Student> students = from s in db.Students.OrderByDescending(s => s.Points).ToList() select s;

            int studRank = students.ToList().IndexOf(student);

            var vm = new RankAndPointsViewModel { Points = student.Points, Rank = studRank + 1, StudentTotal = students.Count() };

            return PartialView(vm);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
           
   
                
                
                
              
                
                
