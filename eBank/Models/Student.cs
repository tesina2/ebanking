using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eBank.Models
{
    public class Student : User
    {
        [Column("ClassroomID")]
        [Required(ErrorMessage = "Required")]
        [ForeignKey("Classroom")]
        public virtual int ClassroomID { get; set; }

        public virtual Classroom Classroom { get; set; }

        //[Column("ExpensesID")]
        //[ForeignKey("Expenses")]
        //public virtual int? ExpensesID { get; set; }

        //public virtual Expenses Expenses { get; set; }

        public virtual ICollection<StudentExpense> StudentExpenses { get; set; }
        
        //Will be implemented later
        public virtual int CreditScore { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }

        [Required(ErrorMessage = "Required")]
        public virtual string EmailAddress { get; set; }

        public virtual bool? bBillsPaid { get; set; }
        public virtual bool? bAlertSetup { get; set; }

        public virtual int Points { get; set; }
        public virtual int? Rank { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "money")]
        public virtual decimal AccountThreshold { get; set; }

        public virtual int? AlertAccountID { get; set; }

        /*//Correct one-to-zero relationship
        public virtual int? BudgetID { get; set; }

        [ForeignKey("BudgetID")]
        public virtual Budget Budget { get; set; }*/

        //Simulation one-to-one relationship
        /*public virtual int? SimulationID { get; set; }

        [ForeignKey("SimulationID")]
        public virtual Simulation Simulation { get; set; }*/

        [Required(ErrorMessage = "Required")]
        public virtual int Happiness { get; set; }
        [Required(ErrorMessage = "Required")]
        public virtual int Health { get; set; }
        [Required(ErrorMessage = "Required")]
        public virtual int Hunger { get; set; }
        [Required(ErrorMessage = "Required")]
        public virtual int Social { get; set; }
        [Required(ErrorMessage = "Required")]
        public virtual int Energy { get; set; }
    }
}